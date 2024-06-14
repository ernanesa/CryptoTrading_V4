using Microsoft.ML;
using Microsoft.ML.Trainers.LightGbm;
using Microsoft.ML.Transforms;
using Recommendation.Data.PostgreSQL;
using Recommendation.Entities;
using System.Diagnostics;

namespace Recommendation.Services
{

    public class TickerTreinamentoService(RecommendationDbContext context, MongoDbService mongoDbService)
    {
        private readonly RecommendationDbContext _context = context;
        private readonly MongoDbService _mongoDbService = mongoDbService;


        #region [ Save ]
        public Task SaveBuyModels()
        {
            const string featureColumnName = "Sell";
            const string labelColumnName = "Buy";

            SaveModels(featureColumnName, labelColumnName);

            return Task.CompletedTask;
        }

        public Task SaveLowModels()
        {
            const string featureColumnName = "High";
            const string labelColumnName = "Low";

            SaveModels(featureColumnName, labelColumnName);

            return Task.CompletedTask;
        }

        public Task SaveSellModels()
        {
            const string featureColumnName = "Buy";
            const string labelColumnName = "Sell";

            SaveModels(featureColumnName, labelColumnName);

            return Task.CompletedTask;
        }

        public Task SaveHighModels()
        {
            const string featureColumnName = "Low";
            const string labelColumnName = "High";

            SaveModels(featureColumnName, labelColumnName);

            return Task.CompletedTask;
        }

        private void SaveModels(string featureColumnName, string labelColumnName)
        {
            var symbols = _context.Symbols.Where(s => s.IsActive).ToList();

            foreach (var symbol in symbols)
            {
                try
                {
                    var stopwatch = Stopwatch.StartNew();
                    stopwatch.Start();

                    var mlContext = new MLContext();
                    var data = LoadIDataView(mlContext, symbol.SymbolName, labelColumnName);
                    var model = Train(mlContext, data, featureColumnName, labelColumnName);
                    SaveModel(mlContext, model, data, symbol.SymbolName, labelColumnName);

                    stopwatch.Stop();
                    Console.WriteLine($"Treinamento Finalizando Save{labelColumnName} {symbol.SymbolName} finalizado em " +
                                      stopwatch.ElapsedMilliseconds + "ms");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erro ao treinar modelo Save{labelColumnName} {symbol.SymbolName}");
                    Console.WriteLine("Mensagem de erro: " + e.Message);
                }
            }

        }
        #endregion

        #region [ Load ]
        private IDataView LoadIDataView(MLContext mlContext, string symbol, string labelColumnName)
        {
            var data = _mongoDbService.GetRecentTickersAsync(symbol, 100000).GetAwaiter().GetResult();

            List<TickerMLInput> list = [];
            switch (labelColumnName)
            {
                case "Buy":
                    list = data.Select(item => new TickerMLInput { Buy = (float)item.Buy }).ToList();
                    break;
                case "Sell":
                    list = data.Select(item => new TickerMLInput { Sell = (float)item.Sell }).ToList();
                    break;
                case "Low":
                    list = data.Select(item => new TickerMLInput { Low = (float)item.Low }).ToList();
                    break;
                case "High":
                    list = data.Select(item => new TickerMLInput { High = (float)item.High }).ToList();
                    break;
            }

            return mlContext.Data.LoadFromEnumerable(list);
        }
        #endregion

        private ITransformer Train(MLContext mlContext, IDataView trainDataView, string featureColumnName, string labelColumnName)
        {
            var pipeline = featureColumnName switch
            {
                "Buy" or "Sell" => BuildPipeline_BS(mlContext, featureColumnName, labelColumnName),
                "Low" or "High" => BuildPipeline_LH(mlContext, featureColumnName, labelColumnName),
                _ => throw new Exception("Erro ao treinar modelo")
            };

            return pipeline.Fit(trainDataView);
        }

        private IEstimator<ITransformer> BuildPipeline_BS(MLContext mlContext, string featureColumnName, string labelColumnName) // Ex.: Se featureColumnName = Buy, labelColumnName = Sell
        {
            return mlContext.Transforms.Categorical.OneHotEncoding(@"Coin", @"Coin", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)
                                        .Append(mlContext.Transforms.ReplaceMissingValues([new InputOutputColumnPair(@"High", @"High"), new InputOutputColumnPair(@"Low", @"Low"), new InputOutputColumnPair(@"Vol", @"Vol"), new InputOutputColumnPair(@"Last", @"Last"), new InputOutputColumnPair(@$"{featureColumnName}", @$"{featureColumnName}"), new InputOutputColumnPair(@"Open", @"Open"), new InputOutputColumnPair(@"Date", @"Date")]))
                                        .Append(mlContext.Transforms.Concatenate(@"Features", [@"Coin", @"High", @"Low", @"Vol", @"Last", @$"{featureColumnName}", @"Open", @"Date"]))
                                        .Append(mlContext.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options() { NumberOfLeaves = 913, NumberOfIterations = 2187, MinimumExampleCountPerLeaf = 20, LearningRate = 0.999999776672986, LabelColumnName = @$"{labelColumnName}", FeatureColumnName = @"Features", Booster = new GradientBooster.Options() { SubsampleFraction = 0.326418837652292, FeatureFraction = 0.99999999, L1Regularization = 4.74758058988521E-10, L2Regularization = 0.710820776614881 }, MaximumBinCountPerFeature = 170 }));
        }

        private IEstimator<ITransformer> BuildPipeline_LH(MLContext mlContext, string featureColumnName, string labelColumnName) // Ex.: Se featureColumnName = Low, labelColumnName = High
        {
            return mlContext.Transforms.Categorical.OneHotEncoding(@"Coin", @"Coin", outputKind: OneHotEncodingEstimator.OutputKind.Indicator)
                                        .Append(mlContext.Transforms.ReplaceMissingValues([new InputOutputColumnPair(@$"{featureColumnName}", @$"{featureColumnName}"), new InputOutputColumnPair(@"Vol", @"Vol"), new InputOutputColumnPair(@"Last", @"Last"), new InputOutputColumnPair(@"Buy", @"Buy"), new InputOutputColumnPair(@"Sell", @"Sell"), new InputOutputColumnPair(@"Open", @"Open"), new InputOutputColumnPair(@"Date", @"Date")]))
                                        .Append(mlContext.Transforms.Concatenate(@"Features", [@"Coin", @$"{featureColumnName}", @"Vol", @"Last", @"Buy", @"Sell", @"Open", @"Date"]))
                                        .Append(mlContext.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options() { NumberOfLeaves = 913, NumberOfIterations = 2187, MinimumExampleCountPerLeaf = 20, LearningRate = 0.999999776672986, LabelColumnName = @$"{labelColumnName}", FeatureColumnName = @"Features", Booster = new GradientBooster.Options() { SubsampleFraction = 0.326418837652292, FeatureFraction = 0.99999999, L1Regularization = 4.74758058988521E-10, L2Regularization = 0.710820776614881 }, MaximumBinCountPerFeature = 170 }));
        }

        private void SaveModel(MLContext mlContext, ITransformer model, IDataView dataView, string symbol, string orderType)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Ticker");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var fileName = Path.Combine($"{symbol}_{orderType}_Model.zip");

                var saveModelPath = Path.Combine(path, fileName);
                mlContext.Model.Save(model, dataView.Schema, saveModelPath);
            }
            catch
            {
                throw;
            }
        }
    }
}
using Microsoft.ML;
using Recommendation.Entities;

namespace Recommendation.Services
{
    public class TickerSugestaoService(MongoDbService mongoDbService)
    {
        private readonly MongoDbService _mongoDbService = mongoDbService;

        public decimal GetBuyValue(string symbol)
        {
            var tickerMlInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMlInput.Buy = (float)ticker.Buy;
            }
            return (decimal)Predict(symbol, "Buy", tickerMlInput).Buy;
        }

        public decimal GetLowValue(string symbol)
        {
            var tickerMlInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMlInput.Low = (float)ticker.Low;
            }
            return (decimal)Predict(symbol, "Low", tickerMlInput).Low;
        }

        public decimal GetSellValue(string symbol)
        {
            var tickerMlInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMlInput.Sell = (float)ticker.Sell;
            }

            return (decimal)Predict(symbol, "Sell", tickerMlInput).Sell;
        }

        public decimal GetHighValue(string symbol)
        {
            var tickerMlInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMlInput.High = (float)ticker.High;
            }

            return (decimal)Predict(symbol, "High", tickerMlInput).High;
        }


        public TickerMLOutput Predict(string symbol, string orderType, TickerMLInput tickerMlInput)
        {
            try
            {
                var PredictEngine = new Lazy<PredictionEngine<TickerMLInput, TickerMLOutput>>(() => CreatePredictEngine(symbol, orderType), true);

                var predEngine = PredictEngine.Value;
                return predEngine.Predict(tickerMlInput);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private PredictionEngine<TickerMLInput, TickerMLOutput> CreatePredictEngine(string symbol, string orderType)
        {
            try
            {
                var mlContext = new MLContext();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Ticker");
                var fileName = Path.Combine($"{symbol}_{orderType}_Model.zip");
                var loadModelPath = Path.Combine(path, fileName);

                var mlModel = mlContext.Model.Load(loadModelPath, out var schema);
                return mlContext.Model.CreatePredictionEngine<TickerMLInput, TickerMLOutput>(mlModel);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
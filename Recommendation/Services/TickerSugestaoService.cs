using Microsoft.ML;
using Recommendation.Entities;

namespace Recommendation.Services
{
    public class TickerSugestaoService(MongoDbService mongoDbService)
    {
        private readonly MongoDbService _mongoDbService = mongoDbService;

        public decimal GetBuyValue(string symbol)
        {
            var tickerMLInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMLInput.Buy = (float)ticker.Buy;
            }
            return (decimal)Predict(symbol, "Buy", tickerMLInput).Buy;
        }

        public decimal GetLowValue(string symbol)
        {
            var tickerMLInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMLInput.Low = (float)ticker.Low;
            }
            return (decimal)Predict(symbol, "Low", tickerMLInput).Low;
        }

        public decimal GetSellValue(string symbol)
        {
            var tickerMLInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMLInput.Sell = (float)ticker.Sell;
            }

            return (decimal)Predict(symbol, "Sell", tickerMLInput).Sell;
        }

        public decimal GetHighValue(string symbol)
        {
            var tickerMLInput = new TickerMLInput();
            var ticker = _mongoDbService.GetRecentTickersAsync(symbol, 1).Result.FirstOrDefault();
            if (ticker != null)
            {
                tickerMLInput.High = (float)ticker.High;
            }

            return (decimal)Predict(symbol, "High", tickerMLInput).High;
        }


        public TickerMLOutput Predict(string symbol, string OrderType, TickerMLInput TickerMLInput)
        {
            try
            {
                var PredictEngine = new Lazy<PredictionEngine<TickerMLInput, TickerMLOutput>>(() => CreatePredictEngine(symbol, OrderType), true);

                var predEngine = PredictEngine.Value;
                return predEngine.Predict(TickerMLInput);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private PredictionEngine<TickerMLInput, TickerMLOutput> CreatePredictEngine(string symbol, string OrderType)
        {
            try
            {
                var mlContext = new MLContext();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Models", "Ticker");
                var fileName = Path.Combine($"{symbol}_{OrderType}_Model.zip");
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
using System.Net;
using DataCollection.Data.MongoDB;
using DataCollection.Data.PostgreSQL;
using DataCollection.Entities;
using DataCollection.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace DataCollection.Tests.UnitTests
{
    public class TickerServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly MongoDbService _mongoDbService;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly DataCollectionDbContext _context;
        private readonly Mock<ILogger<TickerService>> _loggerMock;
        private readonly TickerService _tickerService;

        public TickerServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _context = CreateDbContext();
            _loggerMock = new Mock<ILogger<TickerService>>();

            var mongoDbSettings = Options.Create(new MongoDbSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "test_database",
                TickersCollectionName = "tickers"
            });

            _mongoDbService = new MongoDbService(mongoDbSettings);
            _tickerService = new TickerService(
                _configurationMock.Object,
                _mongoDbService,
                _httpClient,
                _context,
                _loggerMock.Object);
        }

        private DataCollectionDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<DataCollectionDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            return new DataCollectionDbContext(options);
        }

        [Fact]
        public async Task CollectAndSaveTickersAsync_NoSymbols_ReturnsZero()
        {
            // Arrange
            _context.Symbols.RemoveRange(_context.Symbols);  // Ensure the database is empty
            await _context.SaveChangesAsync();

            // Act
            var result = await _tickerService.CollectAndSaveTickersAsync();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CollectAndSaveTickersAsync_UnsuccessfulResponse_ReturnsZero()
        {
            // Arrange
            var symbol = new Symbol { SymbolName = "BTC-BRL" };
            _context.Symbols.Add(symbol);
            await _context.SaveChangesAsync();

            var response = new HttpResponseMessage(HttpStatusCode.BadRequest);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            // Act
            var result = await _tickerService.CollectAndSaveTickersAsync();

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public async Task CollectAndSaveTickersAsync_ExceptionThrown_ReturnsZero()
        {
            // Arrange
            var symbol = new Symbol { SymbolName = "BTC-BRL" };
            _context.Symbols.Add(symbol);
            await _context.SaveChangesAsync();

            var exception = new Exception("An error occurred.");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(exception);

            // Act
            var result = await _tickerService.CollectAndSaveTickersAsync();

            // Assert
            Assert.Equal(0, result);
        }
    }
}

namespace DeliVeggieApplicationNewTest
{
    using Microsoft.AspNetCore.Mvc;
    using RabbitMQ.Client;
    using Moq;
    using DeliVeggieApplication.Interfaces;
    using DeliVeggieApplication.Models;
    using Xunit;

    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<RabbitMqProducer> _mockRabbitMqProducer;
        private readonly ProductController _productController;

        public ProductControllerTests()
        {
            // Mock the IProductService
            _mockProductService = new Mock<IProductService>();

            // Create a mock for IConnection
            _mockConnection = new Mock<IConnection>();

            // Create RabbitMqProducer using the mocked connection
            _mockRabbitMqProducer = new Mock<RabbitMqProducer>(_mockConnection.Object);
            _productController = new ProductController(_mockProductService.Object, _mockRabbitMqProducer.Object);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkWithProductList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = "1", Name = "Product1" },
                new Product { Id = "2", Name = "Product2" }
            };
            _mockProductService.Setup(service => service.GetAllProducts())
                               .ReturnsAsync(products);

            // Act
            var result = await _productController.GetProducts() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnProducts = Assert.IsType<List<Product>>(result.Value);
            Assert.Equal(2, returnProducts.Count);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkWithProduct()
        {
            // Arrange
            var product = new Product { Id = "66dd938b42f642d11b941884", Name = "pile of Potatoes" };
            _mockProductService.Setup(service => service.GetProductById("66dd938b42f642d11b941884"))
                               .ReturnsAsync(product);

            // Act
            var result = await _productController.GetProductById("66dd938b42f642d11b941884") as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var returnProduct = Assert.IsType<Product>(result.Value);
            Assert.Equal("pile of Potatoes", returnProduct.Name);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            _mockProductService.Setup(service => service.GetProductById("66dd938b42f642d11b941884"));

            // Act
            var result = await _productController.GetProductById("66dd938b42f642d11b941884");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}

using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using DeliVeggieApplication.Models;
using DeliVeggieApplication.Interfaces;

namespace DeliVeggieApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<PriceReduction> _reducedPrices;
        private readonly ILogger<ProductService> _logger;
      

        public ProductService(MongoDbContext context, ILogger<ProductService> logger)
        {
            _logger = logger;
            _products = context.GetCollection<Product>("product");
            _reducedPrices = context.GetCollection<PriceReduction>("PriceReductions");
        }

     

        public async Task<List<Product>> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Fetching all products from the database.");
                var productNames = await _products.AsQueryable().ToListAsync();
                return productNames;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching all products.");
                throw; // Rethrow the exception after logging it
            }
        }

        public async Task<Product> GetProductById(string id)
        {
            try
            {
                _logger.LogInformation("Fetching product by ID: {ProductId}", id);
                var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found.", id);
                    return null;
                }

                product.Price = await GetProductWithReducedPriceAsync(id);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching product by ID: {ProductId}", id);
                throw; // Rethrow the exception after logging it
            }
        }

        public async Task<double> GetProductWithReducedPriceAsync(string id)
        {
            try
            {
                _logger.LogInformation("Calculating reduced price for product ID: {ProductId}", id);
                var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

                if (product == null)
                {
                    _logger.LogWarning("Product with ID: {ProductId} not found for price reduction calculation.", id);
                    return 0; // or throw an exception depending on your requirements
                }

                int dayOfWeek = (int)product.EntryDate.DayOfWeek;
                var reduction = await _reducedPrices.Find(r => r.DayOfWeek == dayOfWeek).FirstOrDefaultAsync();

                double newPrice = product.Price;
                if (reduction != null)
                {
                    newPrice -= reduction.Reduction;
                }
                return newPrice;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating reduced price for product ID: {ProductId}", id);
                throw; // Rethrow the exception after logging it
            }
        }
    }
}


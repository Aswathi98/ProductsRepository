using DeliVeggieApplication.Models;

    namespace DeliVeggieApplication.Interfaces
    {
        public interface IProductService
        {
            Task<List<Product>> GetAllProducts();
            Task<Product> GetProductById(string id);
        }

    }


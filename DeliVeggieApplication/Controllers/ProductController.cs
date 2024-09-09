using Microsoft.AspNetCore.Mvc;
using DeliVeggieApplication.Interfaces;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly RabbitMqProducer _rabbitMqProducer;
 

    public ProductController(IProductService productService, RabbitMqProducer rabbitMqProducer)
    {
        _productService = productService;
        _rabbitMqProducer = rabbitMqProducer;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        _rabbitMqProducer.PublishRequest("product_queue", products);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _productService.GetProductById(id);
        _rabbitMqProducer.PublishRequest("product_queue", product);
        return Ok(product);
    }
}

using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;

namespace IMS.Plugins.InMemory;

public class ProductRepository : IProductRepository
{
    private List<Product> _products;

    public ProductRepository()
    {
        _products = new List<Product>()
        {
            new Product { ProductId = 1 , ProductName = "Bike" , Quantity = 10 , Price = 150 },
            new Product { ProductId = 2 , ProductName = "Car" , Quantity = 10 , Price = 25000 },
        };
    }
    
    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return await Task.FromResult(_products);

        return _products
            .Where(x => x.ProductName.Contains(name, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }

    public Task AddProductAsync(Product product)
    {
        if (_products.Any(x =>
                x.ProductName.Equals(product.ProductName, StringComparison.InvariantCultureIgnoreCase)))
        {
            return Task.CompletedTask;
        }
        
        var maxId = _products.Max(x => x.ProductId);
        product.ProductId = maxId + 1;
        
        _products.Add(product);
        
        return Task.CompletedTask;
    }

    public Task UpdateProductAsync(Product product)
    {
        if(_products.Any(x => x.ProductId != product.ProductId 
                                 && x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            return Task.CompletedTask;
        
        var proToUpdate = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

        if (proToUpdate is not null)
        {
            proToUpdate.ProductName = product.ProductName;
            proToUpdate.Quantity = product.Quantity;
            proToUpdate.Price = product.Price;
        }
        
        return Task.CompletedTask;
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        return await Task.FromResult(_products.FirstOrDefault(x => x.ProductId == productId));

    }

    public Task DeleteProductByIdAsync(int productId)
    {
        var proToDelete = _products.FirstOrDefault(x => x.ProductId == productId);

        if (proToDelete is not null)
        {
            _products.Remove(proToDelete);
        }
        return Task.CompletedTask;
    }
}
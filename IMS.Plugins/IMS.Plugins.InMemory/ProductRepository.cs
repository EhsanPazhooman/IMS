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
                                 && x.ProductName.ToLower() == product.ProductName.ToLower())) return Task.CompletedTask;
        
        var prod = _products.FirstOrDefault(x => x.ProductId == product.ProductId);

        if (prod is not null)
        {
            prod.ProductName = product.ProductName;
            prod.Quantity = product.Quantity;
            prod.Price = product.Price;
            prod.ProductInventories = product.ProductInventories;
        }
        return Task.CompletedTask;
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        var product = _products.FirstOrDefault(x => x.ProductId == productId);
        var newProduct = new Product();

        if (product is not null)
        {
            newProduct.ProductId = product.ProductId;
            newProduct.ProductName = product.ProductName;
            newProduct.Quantity = product.Quantity;
            newProduct.Price = product.Price;
            newProduct.ProductInventories = new List<ProductInventory>();
            if (product.ProductInventories != null && product.ProductInventories.Any())
            {
                foreach (var prodInv in product.ProductInventories)
                {
                    var newProductInv = new ProductInventory
                    {
                        InventoryId = prodInv.InventoryId,
                        ProductId = prodInv.ProductId,
                        Product = product,
                        Inventory = new Inventory(),
                        InventoryQuantity = prodInv.InventoryQuantity
                    };
                    if (prodInv.Inventory is not null)
                    {
                         newProductInv.Inventory.InventoryId = prodInv.Inventory.InventoryId;
                         newProductInv.Inventory.InventoryName = prodInv.Inventory.InventoryName;
                         newProductInv.Inventory.Quantity = prodInv.Inventory.Quantity;
                         newProductInv.Inventory.Price = prodInv.Inventory.Price;
                    }
                    newProduct.ProductInventories.Add(newProductInv);
                }
            }
        }
        return await Task.FromResult(newProduct);
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
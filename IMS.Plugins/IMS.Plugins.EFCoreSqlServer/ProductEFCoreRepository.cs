using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCoreSqlServer;

public class ProductEFCoreRepository : IProductRepository
{
    private readonly IDbContextFactory<IMSContext> _contextFactory;

    public ProductEFCoreRepository(IDbContextFactory<IMSContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }


    public async Task<IEnumerable<Product>> GetProductsByNameAsync(string name)
    {
        using var db = _contextFactory.CreateDbContext();
        return await db.Products.Where(x => x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0).ToListAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        using var db = _contextFactory.CreateDbContext();
        db.Products?.Add(product);
        FlagInventoryUnchanged(product, db);
        
        await db.SaveChangesAsync();
    }

    public async Task UpdateProductAsync(Product product)
    {
        using var db = _contextFactory.CreateDbContext();
        var pro = await db.Products.Include(x => x.ProductInventories)
            .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

        if (pro is not null)
        {
            pro.ProductName = product.ProductName;
            pro.Quantity = product.Quantity;
            pro.Price = product.Price;
            pro.ProductInventories = product.ProductInventories;
            FlagInventoryUnchanged(product, db);
            
            await db.SaveChangesAsync();
        }
    }

    public async Task<Product?> GetProductByIdAsync(int productId)
    {
        using var db = _contextFactory.CreateDbContext();
        var product = await db.Products.Include(x => x.ProductInventories)
            .ThenInclude(x => x.Inventory)
            .FirstOrDefaultAsync(x => x.ProductId == productId);
        
        if(product is not null)
            return product;
        
        return new Product();

    }

    public async Task DeleteProductByIdAsync(int productId)
    {
        using var db = _contextFactory.CreateDbContext();
        var product = db.Products?.Find(productId);
        if(product is null) return;
        
        db.Products?.Remove(product);
        await db.SaveChangesAsync();

    }

    private void FlagInventoryUnchanged(Product product, IMSContext db)
    {
        if (product?.ProductInventories != null && product.ProductInventories.Any())
        {
            foreach (var proInv in product.ProductInventories)
            {
                if (proInv.Inventory is not null)
                {
                    db.Entry(proInv.Inventory).State = EntityState.Unchanged;
                }
            }
        }
    }
}
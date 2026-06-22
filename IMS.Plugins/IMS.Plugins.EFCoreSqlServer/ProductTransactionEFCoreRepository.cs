using IMS.CoreBusiness;
using IMS.Plugins.EFCoreSqlServer;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.InMemory;

public class ProductTransactionEFCoreRepository : IProductTransactionRepository
{
    private readonly IDbContextFactory<IMSContext> _contextFactory;
    
    private readonly IProductRepository _productRepository;
    private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
    private readonly IInventoryRepository _inventoryRepository;
    
    public ProductTransactionEFCoreRepository(
        IDbContextFactory<IMSContext> contextFactory,
        IProductRepository productRepository, 
        IInventoryTransactionRepository inventoryTransactionRepository,
        IInventoryRepository inventoryRepository)
    {
        _contextFactory = contextFactory;
        _productRepository = productRepository;
        _inventoryTransactionRepository = inventoryTransactionRepository;
        _inventoryRepository = inventoryRepository;
    }
    public async Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy)
    {
        using var db = _contextFactory.CreateDbContext();
        
        var prod = await _productRepository.GetProductByIdAsync(product.ProductId);

        if (prod is not null)
        {
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.Inventory is not null)
                {
                    _inventoryTransactionRepository.ProduceAsync(
                        productNumber, pi.Inventory, pi.InventoryQuantity * quantity, doneBy, -1);
                    
                    var inv = await _inventoryRepository.GetInventoryByIdAsync(pi.InventoryId);
                    inv.Quantity -= pi.InventoryQuantity * quantity;
                    await _inventoryRepository.UpdateInventoryAsync(inv);
                }
            }
        }
        
        db.ProductTransactions?.Add(new ProductTransaction
        {
            ProductionNumber = productNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            ActivityType = ProductTransactionType.ProduceProduct,
            QuantityAfter = product.Quantity + quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy
        });
        
        await db.SaveChangesAsync();
    }

    public async Task SellProductAsync(string salesOrderNumber, Product product, double unitPrice, int quantity, string doneBy)
    {
        using var db = _contextFactory.CreateDbContext();
        
        db.ProductTransactions?.Add(new ProductTransaction
        {
            ActivityType = ProductTransactionType.SellProduct,
            SoNumber = salesOrderNumber,
            ProductId = product.ProductId,
            QuantityBefore = product.Quantity,
            QuantityAfter = product.Quantity - quantity,
            TransactionDate = DateTime.Now,
            DoneBy = doneBy,
            UnitPrice = unitPrice
        });
        
        await db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductTransaction>> GetProductTransactionAsync(string productName, DateTime? dateFrom, DateTime? dateTo,
        ProductTransactionType? productTransactionType)
    {
        using var db = _contextFactory.CreateDbContext();

        var query = from it in db.ProductTransactions
            join pro in db.Products on it.ProductId equals pro.ProductId
            where
                (string.IsNullOrEmpty(productName) ||
                 pro.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                &&
                (!dateFrom.HasValue || it.TransactionDate >= dateFrom.Value.Date) &&
                (!dateTo.HasValue || it.TransactionDate <= dateTo.Value.Date) &&
                (!productTransactionType.HasValue || it.ActivityType == productTransactionType.Value)
            select it;



        return await query.Include(x => x.Product).ToListAsync();
    }
}
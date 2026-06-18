using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;

public interface IProductTransactionRepository
{
    Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy);
    Task SellProductAsync(string salesOrderNumber, Product product, double unitPrice, int quantity, string doneBy);
    Task<IEnumerable<ProductTransaction>> GetProductTransactionAsync(string productName, DateTime? dateFrom, 
        DateTime? dateTo, ProductTransactionType? productTransactionType);
}
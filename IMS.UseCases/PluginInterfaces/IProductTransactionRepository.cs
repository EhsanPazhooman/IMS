using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;

public interface IProductTransactionRepository
{
    Task ProduceAsync(string productNumber, Product product, int quantity, string doneBy);
}
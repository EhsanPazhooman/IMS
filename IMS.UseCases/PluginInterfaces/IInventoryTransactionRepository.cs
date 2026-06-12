using IMS.CoreBusiness;

namespace IMS.UseCases.PluginInterfaces;

public interface IInventoryTransactionRepository
{
    void PurchaseAsync(string poNumber, Inventory inventory, int quantity, string doneBy, double price);

    void ProduceAsync(string producionNumber, Inventory inventory, int quantityToConsume, string doneBy, double price);
}
using IMS.CoreBusiness;

namespace IMS.UseCases.Reports.Interfaces;

public interface ISearchInventoryTransactionUseCase
{
    Task<IEnumerable<InventoryTransaction>> ExecuteAsync(string inventoryName, DateTime? datefrom, DateTime? dateTo, 
        InventoryTransactionType? inventoryTransactionType);
}
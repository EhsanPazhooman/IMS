using IMS.CoreBusiness;

namespace IMS.UseCases.Activities.Interfaces;

public interface ISellProductUseCase
{ 
    Task ExecuteAsync(string salesOrderNumber, Product product, double unitPrice, int quantity, string doneBy);
}
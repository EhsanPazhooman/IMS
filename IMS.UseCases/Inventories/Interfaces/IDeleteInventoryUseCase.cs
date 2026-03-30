namespace IMS.UseCases.Inventories;

public interface IDeleteInventoryUseCase
{
    Task ExecuteAsync(int productId);
}
using IMS.CoreBusiness;
using IMS.UseCases.Products.Interfaces;
using IMS.UseCases.PluginInterfaces;

namespace IMS.UseCases.Products;

public class ViewProductsByNameUseCase : IViewProductsByNameUseCase
{
    private readonly IProductRepository _productsRepository;

    public ViewProductsByNameUseCase(IProductRepository productRepository)
    {
        _productsRepository = productRepository;
    }
    
    public async Task<IEnumerable<Product>> ExecuteAsync(string name = "")
    {
        return await _productsRepository.GetProductsByNameAsync(name);
    }
}

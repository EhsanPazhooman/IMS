using System.ComponentModel.DataAnnotations;
using IMS.CoreBusiness;
using IMS.WebApp.viewModelsValidations;

namespace IMS.WebApp.ViewModels;

public class ProduceViewModel
{
    [Required]
    public string ProductionNumber { get; set; } = string.Empty;
    
    [Range(minimum:1, maximum:int.MaxValue , ErrorMessage = "You have to select a product.")]
    public int ProductId { get; set; }

    [Range(minimum:1, maximum:int.MaxValue , ErrorMessage = "Quantity must be greater than 0.")]
    [Produce_EnsureEnoughInventoryQuantity]
    public int QuantityToProduce { get; set; }

    public Product? Product { get; set; } = null;
}
using System.ComponentModel.DataAnnotations;
using IMS.CoreBusiness;
using IMS.WebApp.viewModelsValidations;

namespace IMS.WebApp.ViewModels;

public class SellViewModel
{
    [Required]
    public string SalesOrderNumber { get; set; } = String.Empty;
    
    [Range(1, int.MaxValue, ErrorMessage = "You have to select a product")]
    public int ProductId { get; set; }
    
    
    [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be greater than 0")]
    [Sell_EnsureEnoughProductQuantity]
    public int QuantityToSell { get; set; }
    
    [Range(0, int.MaxValue, ErrorMessage = "Price has to be greater than 0")]
    public double UnitPrice { get; set; }
    
    public Product? Product { get; set; }
}
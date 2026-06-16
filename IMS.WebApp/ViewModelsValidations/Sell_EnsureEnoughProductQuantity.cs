using System.ComponentModel.DataAnnotations;
using IMS.WebApp.ViewModels;

namespace IMS.WebApp.viewModelsValidations;

public class Sell_EnsureEnoughProductQuantity : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var sellViewModel = validationContext.ObjectInstance as SellViewModel;

        if (sellViewModel is not null)
        {
            if (sellViewModel.Product.Quantity < sellViewModel.QuantityToSell)
            {
                return new ValidationResult($"There is only {sellViewModel.Product.Quantity} items to sell",
                   new [] {validationContext.MemberName});
            }
        }
        return ValidationResult.Success;
    }
    
}
using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness;

public class Product
{
    public int ProductId { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string ProductName { get; set; } = string.Empty;
    
    [Range(0,int.MaxValue,ErrorMessage ="Please enter a number greater or equal to 0.")]
    public int Quantity { get; set; }
    
    [Range(0,int.MaxValue,ErrorMessage ="Please enter a number greater or equal to 0.")]
    public double Price { get; set; }
}
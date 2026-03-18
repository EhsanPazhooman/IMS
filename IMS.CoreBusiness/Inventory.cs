using System.ComponentModel.DataAnnotations;

namespace IMS.CoreBusiness;

public class Inventory
{
    public int InventoryId { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string InventoryName { get; set; } = string.Empty;
    
    [Range(0,int.MaxValue,ErrorMessage ="Please enter a number greater or equal to 0.")]
    public int Quantity { get; set; }
    
    [Range(0,int.MaxValue,ErrorMessage ="Please enter a number greater or equal to 0.")]
    public double Price { get; set; }
}
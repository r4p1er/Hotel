using System.ComponentModel.DataAnnotations;

namespace Managing.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Summary { get; set; }
    
    [MaxLength(255)]
    public string Description { get; set; }
    
    public decimal Price { get; set; }
}
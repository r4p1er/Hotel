using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure.Database.Entities;

public class Role
{
    public int Id { get; set; }
    
    [MaxLength(255)]
    public string Alias { get; set; }
    
    [MaxLength(255)]
    public string Description { get; set; }
}
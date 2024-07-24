using System.ComponentModel.DataAnnotations;
using Identity.Domain.Enums;

namespace Identity.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    [MaxLength(255)]
    public string Name { get; set; }
    
    [MaxLength(255)]
    public string Surname { get; set; }
    
    [MaxLength(255)]
    public string? Patronymic { get; set; }
    
    [MaxLength(50)]
    public string Email { get; set; }
    
    [MaxLength(14)]
    public string PhoneNumber { get; set; }
    
    [MaxLength(255)]
    public string PasswordHash { get; set; }
    
    public Role Role { get; set; }

    public bool IsBlocked { get; set; } = false;
}
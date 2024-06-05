using System.ComponentModel.DataAnnotations;

namespace Identity.Infrastructure.Database.Entities;

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
    
    public int RoleId { get; set; }
    
    public Role Role { get; set; }

    public bool IsBlocked { get; set; } = false;
}
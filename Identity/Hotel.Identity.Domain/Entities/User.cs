using System.ComponentModel.DataAnnotations;
using Hotel.Identity.Domain.Enums;

namespace Hotel.Identity.Domain.Entities;

/// <summary>
/// Пользователь
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя пользователя
    /// </summary>
    [MaxLength(255)]
    public string Name { get; set; }
    
    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    [MaxLength(255)]
    public string Surname { get; set; }
    
    /// <summary>
    /// Отчество пользователя
    /// </summary>
    [MaxLength(255)]
    public string? Patronymic { get; set; }
    
    /// <summary>
    /// Электронная почта пользователя
    /// </summary>
    [MaxLength(50)]
    public string Email { get; set; }
    
    /// <summary>
    /// Телефонный номер пользователя
    /// </summary>
    [MaxLength(14)]
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Хэш пароля пользователя
    /// </summary>
    [MaxLength(255)]
    public string PasswordHash { get; set; }
    
    /// <inheritdoc cref="Role"/>
    public Role Role { get; set; }

    /// <summary>
    /// Заблокирован ли пользователь
    /// </summary>
    public bool IsBlocked { get; set; } = false;
}
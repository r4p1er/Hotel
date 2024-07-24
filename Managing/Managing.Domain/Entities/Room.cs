using System.ComponentModel.DataAnnotations;

namespace Managing.Domain.Entities;

/// <summary>
/// Номер
/// </summary>
public class Room
{
    /// <summary>
    /// Идентификатор номера
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название номера
    /// </summary>
    [MaxLength(255)]
    public string Summary { get; set; }
    
    /// <summary>
    /// Описание номера
    /// </summary>
    [MaxLength(255)]
    public string Description { get; set; }
    
    /// <summary>
    /// Цена номера
    /// </summary>
    public decimal Price { get; set; }
}
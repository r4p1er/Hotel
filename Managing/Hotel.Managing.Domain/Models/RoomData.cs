namespace Hotel.Managing.Domain.Models;

/// <summary>
/// Данные для создания нового номера
/// </summary>
public class RoomData
{
    /// <summary>
    /// Название номера
    /// </summary>
    public string Summary { get; set; }
    
    /// <summary>
    /// Описание номера
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Цена номера
    /// </summary>
    public decimal Price { get; set; }
}
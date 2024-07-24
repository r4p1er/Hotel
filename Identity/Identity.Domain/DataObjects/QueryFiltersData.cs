using Identity.Domain.Enums;

namespace Identity.Domain.DataObjects;

/// <summary>
/// Фильтры запроса
/// </summary>
public class QueryFiltersData
{
    /// <summary>
    /// Поиск по строке
    /// </summary>
    public string? Search { get; set; }
    
    /// <summary>
    /// Смещение в коллекции
    /// </summary>
    public int? Offset { get; set; }
    
    /// <summary>
    /// Количество выбираемых из коллекци объектов
    /// </summary>
    public int? Limit { get; set; }
    
    /// <inheritdoc cref="SortOrder"/>
    public SortOrder? SortOrder { get; set; }
    
    /// <summary>
    /// Поле, по которому произвести сортировку
    /// </summary>
    public string? SortBy { get; set; }
}
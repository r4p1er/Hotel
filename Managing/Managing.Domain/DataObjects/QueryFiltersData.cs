using Managing.Domain.Enums;

namespace Managing.Domain.DataObjects;

/// <summary>
/// Фильтры для запроса
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
    /// Количество выбираемых объектов из коллекции
    /// </summary>
    public int? Limit { get; set; }
    
    /// <inheritdoc cref="SortOrder"/>
    public SortOrder? SortOrder { get; set; }
    
    /// <summary>
    /// Поле, по которому следует сортировать
    /// </summary>
    public string? SortBy { get; set; }
}
using Identity.Domain.Enums;

namespace Identity.Domain.DataObjects;

public class QueryFiltersData
{
    public string? Search { get; set; }
    
    public int? Offset { get; set; }
    
    public int? Limit { get; set; }
    
    public SortOrder? SortOrder { get; set; }
    
    public string? SortBy { get; set; }
}
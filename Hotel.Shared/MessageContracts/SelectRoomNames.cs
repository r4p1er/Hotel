namespace Hotel.Shared.MessageContracts;

/// <summary>
/// Команда на выборку названий комнат
/// </summary>
public class SelectRoomNames
{
    /// <summary>
    /// Лист с идентификаторами комнат
    /// </summary>
    public List<Guid> Guids { get; set; }
}
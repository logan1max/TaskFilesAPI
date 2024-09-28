namespace TaskFilesAPI.DTO;

/// <summary>
/// Ошибка обработки входящих данных
/// </summary>
public class Error
{
    /// <summary>
    /// Описание ошибки.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}

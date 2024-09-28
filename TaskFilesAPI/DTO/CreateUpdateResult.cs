namespace TaskFilesAPI.DTO;

/// <summary>
/// Результат создания или обновления объекта.
/// </summary>
public class CreateUpdateResult
{
    /// <summary>
    /// Внутренний идентификатор
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Ошибки.
    /// </summary>
    public IReadOnlyCollection<Error>? Errors { get; set; }
}

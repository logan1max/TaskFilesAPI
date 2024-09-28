namespace TaskFilesAPI.DTO;

/// <summary>
/// Модель задачи при создании или обновлении
/// </summary>
public class TaskCreateUpdateModel
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public Guid? TaskId { get; set; }

    /// <summary>
    /// Наименование задачи
    /// </summary>
    public string? Name { get; set; } = null!;

    /// <summary>
    /// Статус выполнения
    /// </summary>
    public string? Status { get; set; } = null!;

    /// <summary>
    /// Файлы
    /// </summary>
    public IFormFileCollection? Files { get; set; }
}

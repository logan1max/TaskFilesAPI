namespace TaskFilesAPI.Contracts;

public class TaskModel
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// Наименование задачи
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Статус выполнения
    /// </summary>
    public string Status { get; set; } = null!;

    /// <summary>
    /// Автор изменения записи
    /// </summary>
    public string RowChangedBy { get; set; } = null!;

    /// <summary>
    /// Дата изменения записи
    /// </summary>
    public DateTime RowChangedDate { get; set; }

    /// <summary>
    /// Автор создания записи
    /// </summary>
    public string RowCreatedBy { get; set; } = null!;

    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTime RowCreatedDate { get; set; }

    /// <summary>
    /// Файлы
    /// </summary>
    public List<FileModel> Files { get; set; } = [];
}

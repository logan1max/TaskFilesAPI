namespace TaskFilesAPI.DataAccess.Context.Entities;

[Table("task")]
[Comment("Задача")]
public class TaskModel
{
    public TaskModel()
    {
        Files = new HashSet<FileModel>();
    }

    [Key]
    [Comment("Идентификатор задачи")]
    public Guid TaskId { get; set; }

    [Comment("Наименование задачи")]
    public string Name { get; set; } = null!;

    [Comment("Статус выполнения")]
    public string Status { get; set; } = null!;

    [Comment("Автор изменения записи")]
    public string RowChangedBy { get; set; } = null!;

    [Comment("Дата изменения записи")]
    public DateTime RowChangedDate { get; set; }

    [Comment("Автор создания записи")]
    public string RowCreatedBy { get; set; } = null!;

    [Comment("Дата создания записи")]
    public DateTime RowCreatedDate { get; set; }

    public virtual ICollection<FileModel> Files { get; set; }
}

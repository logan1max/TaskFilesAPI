namespace TaskFilesAPI.DataAccess.Context.Entities;

[Table("file")]
[Comment("Файл")]
public class FileModel
{
    [Key]
    [Comment("Идентификатор файла")]
    public Guid FileId { get; set; }

    [Comment("Идентификатор задачи")]
    public Guid TaskId { get; set; }

    [Comment("Имя файла")]
    public string Name { get; set; } = null!;

    [Comment("Дата загрузки файла")]
    public DateTime UploadDate { get; set; }

    [Comment("Размер файла")]
    public long Length { get; set; }

    public virtual TaskModel Task { get; set; } = null!;
}

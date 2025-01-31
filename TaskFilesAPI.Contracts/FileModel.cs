﻿namespace TaskFilesAPI.Contracts;

public class FileModel
{
    /// <summary>
    /// Идентификатор файла
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public Guid TaskId { get; set; }

    /// <summary>
    /// Имя файла
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Дата загрузки файла
    /// </summary>
    public DateTime UploadDate { get; set; }

    /// <summary>
    /// Размер файла
    /// </summary>
    public long Length { get; set; }

    /// <summary>
    /// Тип контента файла
    /// </summary>
    public string ContentType { get; set; } = null!;
}

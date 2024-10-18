using System.IO.Compression;
using ChinhDo.Transactions;
using Microsoft.IdentityModel.Tokens;
using TaskFilesAPI.Contracts;

namespace TaskFilesAPI.Services.Helpers;

public class FileHelper : IFileHelper
{
    private const string UploadFilesFolderName = "UploadFiles";
    private const string TempFolderName = "Temp";

    private const string FolderNotFound = "Каталог не найден";

    public async Task<byte[]> GetFilesAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var path = GetTaskFilesPath(taskId);

        if (!Directory.Exists(path))
        {
            return [];
        }

        var dirInfo = new DirectoryInfo(path);

        foreach (var file in dirInfo.GetFiles())
        {
            return await File.ReadAllBytesAsync(file.FullName, cancellationToken);
        }

        return [];
    }

    public async Task<List<FileModel>> UploadTaskFilesAsync(Guid taskId, IFormFileCollection? files, CancellationToken cancellationToken)
    {
        var result = new List<FileModel>();

        if (files.IsNullOrEmpty())
        {
            return result;
        }

        foreach (var file in files!)
        {
            result.Add(await WriteFileToFolderAsync(taskId, file, cancellationToken));
        }

        return result;
    }

    public List<OperationResult<Guid>> DeleteTasksFiles(List<Guid> taskIds, IFileManager fm)
    {
        var result = new List<OperationResult<Guid>>();

        foreach (var taskId in taskIds)
        {
            var path = GetTaskFilesPath(taskId);

            if (!Directory.Exists(path))
            {
                result.Add(new OperationResult<Guid>
                {
                    Subject = taskId,
                    Errors = [FolderNotFound,]
                });

                continue;
            }

            fm.DeleteDirectory(path);

            result.Add(new OperationResult<Guid>
            {
                Subject = taskId,
            });
        }

        return result;
    }

    private static async Task<FileModel> WriteFileToFolderAsync(Guid taskId, IFormFile file, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(file.FileName);
        var path = GetTaskFilesPath(taskId);

        Directory.CreateDirectory(path);

        var fileId = Guid.NewGuid();

        using var stream = new FileStream(Path.Combine(path, fileId.ToString() + extension), FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        var result = new FileModel()
        {
            TaskId = taskId,
            FileId = fileId,
            Name = file.FileName,
            Length = file.Length,
            UploadDate = DateTime.UtcNow,
            ContentType = file.ContentType,
        };

        return result;
    }

    private static string GetTaskFilesPath(Guid taskId) => Path.Combine(Directory.GetCurrentDirectory(), UploadFilesFolderName, taskId.ToString());
}

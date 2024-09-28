using ChinhDo.Transactions;
using TaskFilesAPI.Contracts;

namespace TaskFilesAPI.Services.Helpers;

public interface IFileHelper
{
    Task<byte[]> GetFilesAsync(Guid taskId, CancellationToken cancellationToken);
    Task<List<FileModel>> UploadTaskFilesAsync(Guid taskId, IFormFileCollection? files, CancellationToken cancellationToken);
    List<OperationResult<Guid>> DeleteTasksFiles(List<Guid> taskIds, IFileManager fm);
    Task<string> CreateZipFilesBase64Async(Guid taskId, CancellationToken cancellationToken);
}

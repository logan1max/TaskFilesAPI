using TaskFilesAPI.DTO;

namespace TaskFilesAPI.Services;

public interface IFileService
{
    Task<FileUploadSummary> UploadFilesAsync(Guid taskId, Stream fileStream, string contentType, CancellationToken cancellationToken);
}

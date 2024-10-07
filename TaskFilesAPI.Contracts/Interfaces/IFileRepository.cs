namespace TaskFilesAPI.Contracts.Interfaces;

public interface IFileRepository
{
    Task<List<OperationResult<FileModel>>> CreateFilesAsync(Guid taskId, List<FileModel> fileModels, CancellationToken cancellationToken);
    Task<List<OperationResult<Guid>>> DeleteFilesAsync(List<Guid> modelIds, CancellationToken cancellationToken);
}

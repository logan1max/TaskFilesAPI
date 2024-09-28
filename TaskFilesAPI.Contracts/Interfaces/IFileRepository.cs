namespace TaskFilesAPI.Contracts.Interfaces;

public interface IFileRepository
{
    Task<List<OperationResult<Guid>>> DeleteTasksFilesAsync(List<Guid> modelIds, CancellationToken cancellationToken);
}

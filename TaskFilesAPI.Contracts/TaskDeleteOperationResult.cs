namespace TaskFilesAPI.Contracts;

public class TaskDeleteOperationResult
{
    public List<OperationResult<Guid>> Tasks { get; init; } = null!;
    public List<OperationResult<Guid>> Files { get; init; } = null!;
    public List<OperationResult<Guid>> FolderFiles { get; init; } = null!;
}

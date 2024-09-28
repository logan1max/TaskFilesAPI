namespace TaskFilesAPI.DTO;

public class TaskDeleteOperationResult
{
    public List<CreateUpdateResult> Tasks { get; init; } = null!;
    public List<CreateUpdateResult> Files { get; init; } = null!;
    public List<CreateUpdateResult> FolderFiles { get; init; } = null!;
}

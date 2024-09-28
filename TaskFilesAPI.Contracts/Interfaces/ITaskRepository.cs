namespace TaskFilesAPI.Contracts.Interfaces;

public interface ITaskRepository
{
    Task<TaskModel> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task CreateTaskAsync(TaskModel task, string user, CancellationToken cancellationToken);
    Task CreateTasksAsync(List<TaskModel> tasks, CancellationToken cancellationToken);
    Task UpdateTaskAsync(TaskModel task, CancellationToken cancellationToken);
    Task<List<OperationResult<Guid>>> DeleteTasksAsync(List<Guid> modelIds, CancellationToken cancellationToken);
}

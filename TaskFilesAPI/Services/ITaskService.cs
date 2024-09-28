﻿using TaskFilesAPI.Contracts;
using TaskFilesAPI.DTO;

namespace TaskFilesAPI.Services;

public interface ITaskService
{
    Task<TaskModel> GetByIdAsync(Guid taskId, bool includeFiles, CancellationToken cancellationToken);
    Task<byte[]> GetTaskFileBytesAsync(Guid taskId, CancellationToken cancellationToken);
    Task CreateTasksAsync(List<TaskCreateUpdateModel> tasks, CancellationToken cancellationToken);
    Task CreateTaskAsync(TaskCreateUpdateModel task, string user, CancellationToken cancellationToken);
    Task UpdateTaskAsync(TaskCreateUpdateModel task, CancellationToken cancellationToken);
    Task<Contracts.TaskDeleteOperationResult> DeleteTasksAsync(List<Guid> taskIds, CancellationToken cancellationToken);
}

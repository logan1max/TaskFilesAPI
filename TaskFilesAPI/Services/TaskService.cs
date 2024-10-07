using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using ChinhDo.Transactions;
using TaskFilesAPI.Contracts;
using TaskFilesAPI.Contracts.Exceptions;
using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DTO;
using TaskFilesAPI.Services.Helpers;

namespace TaskFilesAPI.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IFileHelper _fileHelper; 
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository taskRepository, 
        IFileRepository fileRepository, 
        IFileHelper fileHelper, 
        IMapper mapper)
    {
        _taskRepository = taskRepository;
        _fileRepository = fileRepository;
        _fileHelper = fileHelper;
        _mapper = mapper;
    }

    public async Task<TaskModel> GetByIdAsync(Guid taskId, bool includeFiles, CancellationToken cancellationToken)
    {
        var task = await _taskRepository.GetTaskByIdAsync(taskId, cancellationToken);

        return task;
    }

    public async Task<byte[]> GetTaskFileBytesAsync(Guid taskId, CancellationToken cancellationToken)
    {
        return await _fileHelper.GetFilesAsync(taskId, cancellationToken);
    }

    public async Task CreateTasksAsync(List<TaskCreateUpdateModel> tasks, CancellationToken cancellationToken)
    {
        var result = new List<TaskModel>();

        foreach (var task in tasks)
        {
            var taskId = task.TaskId ?? Guid.NewGuid();

            var files = await _fileHelper.UploadTaskFilesAsync(taskId, task.Files, cancellationToken);

            var taskItem = new TaskModel()
            {
                TaskId = taskId,
                Name = task.Name ?? string.Empty,
                Status = task.Status ?? string.Empty,
                Files = files,
            };

            result.Add(taskItem);
        }

        //await _taskRepository.CreateTaskAsync(result, cancellationToken);
    }

    public async Task CreateTaskAsync(TaskCreateUpdateModel task, string user, CancellationToken cancellationToken)
    {
        var taskId = task.TaskId ?? Guid.NewGuid();

        var files = await _fileHelper.UploadTaskFilesAsync(taskId, task.Files, cancellationToken);

        var result = new TaskModel()
        {
            TaskId = taskId,
            Name = task.Name ?? string.Empty,
            Status = task.Status ?? string.Empty,
            Files = files,
        };

        await _taskRepository.CreateTaskAsync(result, user, cancellationToken);
    }

    public async Task UpdateTaskAsync(TaskCreateUpdateModel task, CancellationToken cancellationToken)
    {
        await _taskRepository.GetTaskByIdAsync(task.TaskId!.Value, cancellationToken);

        //if (task.TaskId)
        //{

        //}
    }

    public async Task<Contracts.TaskDeleteOperationResult> DeleteTasksAsync(List<Guid> taskIds, CancellationToken cancellationToken)
    {
        IFileManager fm = new TxFileManager();

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var filesOperationResult = await _fileRepository.DeleteFilesAsync(taskIds, cancellationToken);
        var tasksOperationResult = await _taskRepository.DeleteTasksAsync(taskIds, cancellationToken);
        var folderFilesOperationResult = _fileHelper.DeleteTasksFiles(taskIds, fm);

        var result = new Contracts.TaskDeleteOperationResult()
        {
            Files = filesOperationResult,
            Tasks = tasksOperationResult,
            FolderFiles = folderFilesOperationResult,
        };

        scope.Complete();

        return result;
    }
}

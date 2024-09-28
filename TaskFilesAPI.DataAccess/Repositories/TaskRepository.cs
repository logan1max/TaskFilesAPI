using AutoMapper;
using TaskFilesAPI.Contracts.Exceptions;
using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DataAccess.Context;
using TaskFilesAPI.DataAccess.Context.Entities;

namespace TaskFilesAPI.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private const string NotFound = "Объект не найден";

    private readonly TaskFilesContext _context;
    private readonly IFileRepository _fileRepository;
    private readonly IMapper _mapper;

    public TaskRepository(TaskFilesContext context, IFileRepository fileRepository, IMapper mapper)
    {
        _context = context;
        _fileRepository = fileRepository;
        _mapper = mapper;
    }

    public async Task<Contracts.TaskModel> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks
            .Where(entity => entity.TaskId == taskId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) 
            ?? throw new TaskNotFoundException(taskId);

        return _mapper.Map<Contracts.TaskModel>(task);
    }

    public async Task CreateTaskAsync(Contracts.TaskModel task, string user, CancellationToken cancellationToken)
    {
        var mappedTask = _mapper.Map<TaskModel>(task);

        mappedTask.RowCreatedBy = user;
        mappedTask.RowCreatedDate = DateTime.UtcNow;
        mappedTask.RowChangedBy = user;
        mappedTask.RowChangedDate = DateTime.UtcNow;

        await _context.AddAsync(mappedTask, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateTasksAsync(List<Contracts.TaskModel> tasks, CancellationToken cancellationToken)
    {
        var mappedTasks = _mapper.Map<List<TaskModel>>(tasks);

        await _context.AddRangeAsync(mappedTasks, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public Task UpdateTaskAsync(Contracts.TaskModel task, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Contracts.OperationResult<Guid>>> DeleteTasksAsync(List<Guid> modelIds, CancellationToken cancellationToken)
    {
        var results = new List<Contracts.OperationResult<Guid>>();

        var existingEntities = await _context.Tasks
            .Where(entity => modelIds.Contains(entity.TaskId))
            .ToDictionaryAsync(entity => entity.TaskId, cancellationToken);

        foreach (var modelId in modelIds)
        {
            var entityFound = existingEntities.TryGetValue(modelId, out var entity);

            if (entityFound)
            {
                _context.Set<TaskModel>().Remove(entity!);

                results.Add(new Contracts.OperationResult<Guid>
                {
                    Subject = modelId
                });
            }
            else
            {
                results.Add(new Contracts.OperationResult<Guid>
                {
                    Subject = modelId,
                    Errors = [NotFound,]
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return results;

        //if (existingEntity == null)
        //{
        //    //throw new Exception();
        //}

        //_context.Tasks.Remove(existingEntity!);
        //await _context.SaveChangesAsync(cancellationToken);
    }

    internal async Task<IReadOnlyCollection<Contracts.OperationResult<Guid>>> DeleteAsync<TEntity>(
        IReadOnlyCollection<Guid> modelIds,
        IDictionary<Guid, TEntity> existingEntitiesMap,
        CancellationToken cancellationToken)
        where TEntity : class
    {
        var results = new List<Contracts.OperationResult<Guid>>();

        foreach (var modelId in modelIds)
        {
            var entityFound = existingEntitiesMap.TryGetValue(modelId, out var entity);

            if (entityFound)
            {
                _context.Set<TEntity>().Remove(entity!);

                results.Add(new Contracts.OperationResult<Guid>
                {
                    Subject = modelId
                });
            }
            else
            {
                results.Add(new Contracts.OperationResult<Guid>
                {
                    Subject = modelId,
                    Errors = [NotFound,]
                });
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return results;
    }
}

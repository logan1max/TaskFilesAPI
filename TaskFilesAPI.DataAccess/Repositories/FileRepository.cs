using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DataAccess.Context;
using TaskFilesAPI.DataAccess.Context.Entities;

namespace TaskFilesAPI.DataAccess.Repositories;

public class FileRepository : IFileRepository
{
    private const string NotFound = "Объект не найден";

    private readonly TaskFilesContext _context;

    public FileRepository(TaskFilesContext context)
    {
        _context = context;
    }

    public async Task<List<Contracts.OperationResult<Guid>>> DeleteTasksFilesAsync(List<Guid> modelIds, CancellationToken cancellationToken)
    {
        var results = new List<Contracts.OperationResult<Guid>>();

        var existingEntities = await _context.Files
            .Where(entity => modelIds.Contains(entity.TaskId))
            .GroupBy(entity => entity.TaskId)
            .ToDictionaryAsync(gr => gr.Key, cancellationToken);

        foreach (var modelId in modelIds)
        {
            var entityFound = existingEntities.TryGetValue(modelId, out var entity);

            if (entityFound)
            {
                var taskFiles = entity!
                    .Select(gr => gr)
                    .ToList();

                _context.Files.RemoveRange(taskFiles);

                //taskFiles
                //    .ForEach(tf => results.Add(new Contracts.OperationResult<Guid> 
                //    { 
                //        Subject = tf.FileId 
                //    }));

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

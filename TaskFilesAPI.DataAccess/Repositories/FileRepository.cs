using AutoMapper;
using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DataAccess.Context;
using TaskFilesAPI.DataAccess.Context.Entities;

namespace TaskFilesAPI.DataAccess.Repositories;

public class FileRepository : IFileRepository
{
    private const string NotFound = "Объект не найден";

    private readonly TaskFilesContext _context;
    private readonly IMapper _mapper;

    public FileRepository(TaskFilesContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<Contracts.OperationResult<Contracts.FileModel>>> CreateFilesAsync(Guid taskId, List<Contracts.FileModel> fileModels, CancellationToken cancellationToken)
    {
        var mappedFiles = _mapper.Map<List<FileModel>>(fileModels);

        await _context.AddRangeAsync(mappedFiles, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var result = new List<Contracts.OperationResult<Contracts.FileModel>>();

        foreach(var f in mappedFiles)
        {
            result.Add(new Contracts.OperationResult<Contracts.FileModel>
            {
                Subject = _mapper.Map<Contracts.FileModel>(f),
                Errors = [],
            });
        }

        return result;
    }

    public async Task<List<Contracts.OperationResult<Guid>>> DeleteFilesAsync(List<Guid> modelIds, CancellationToken cancellationToken)
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

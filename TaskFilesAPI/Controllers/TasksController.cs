using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskFilesAPI.DTO;
using TaskFilesAPI.Services;

namespace TaskFilesAPI.Controllers;

/// <summary>
/// Задачи
/// </summary>
[Route("api/tasks")]
[ApiController]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public TasksController(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetById(Guid taskId, bool includeFiles, CancellationToken cancellationToken)
    {
        var task = await _taskService.GetByIdAsync(taskId, includeFiles, cancellationToken);
        var file = await _taskService.GetTaskFileBytesAsync(taskId, cancellationToken);

        var result = new
        {
            Task = task,
            FileResponse = File(file, "text/plain"),
        };

        return Ok(result);
    }

    //[HttpPost]
    //[Route("tasks")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //public async Task<ActionResult> CreateTasks(List<TaskCreateUpdateModel> tasks, CancellationToken cancellationToken)
    //{
    //    await _taskService.CreateTasksAsync(tasks, cancellationToken);

    //    return Created();
    //}

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> CreateSingleTask([FromForm] TaskCreateUpdateModel task, string user, CancellationToken cancellationToken)
    {
        await _taskService.CreateTaskAsync(task, user, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [ProducesResponseType(typeof(TaskDeleteOperationResult), StatusCodes.Status200OK)]
    public async Task<ActionResult> Delete(List<Guid> taskIds, CancellationToken cancellationToken)
    {
        var results = await _taskService.DeleteTasksAsync(taskIds, cancellationToken);

        return Ok(_mapper.Map<TaskDeleteOperationResult>(results));
    }
}

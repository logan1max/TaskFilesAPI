namespace TaskFilesAPI.Contracts.Exceptions;

public class TaskNotFoundException(Guid taskId) : ResourceNotFoundException($"Задача с id = '{taskId}' не найдена")
{
}

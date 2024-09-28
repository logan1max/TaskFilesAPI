namespace TaskFilesAPI.Contracts.Exceptions;

public class ResourceNotFoundException(string? message) : AppException(message)
{
}

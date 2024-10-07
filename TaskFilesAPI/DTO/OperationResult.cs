namespace TaskFilesAPI.DTO;

/// <summary>
/// Результат операции с указанием объекта.
/// </summary>
public class OperationResult<T>
{
    public T Subject { get; init; } = default!;
    public IReadOnlyCollection<string>? Errors { get; init; }
    public bool Success => Errors == null || !(Errors.Count != 0);
}

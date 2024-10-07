namespace TaskFilesAPI.Contracts.Exceptions;

public class UploadFileStreamIsNullException(string fileName) : AppException($"Сбой при обработке файлового потока для файла {fileName}")
{
}

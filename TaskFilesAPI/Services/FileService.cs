using System.Transactions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using TaskFilesAPI.Contracts;
using TaskFilesAPI.Contracts.Exceptions;
using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DTO;

namespace TaskFilesAPI.Services;

public class FileService : IFileService
{
    private const string UploadsSubDirectory = "FilesUploaded";

    private readonly IFileRepository _fileRepository;
    private readonly ITaskRepository _taskRepository;

    public FileService(IFileRepository fileRepository, ITaskRepository taskRepository)
    {
        _fileRepository = fileRepository;
        _taskRepository = taskRepository;
    }

    public async Task<FileUploadSummary> UploadFilesAsync(Guid taskId, Stream fileStream, string contentType, CancellationToken cancellationToken)
    {
        await _taskRepository.GetTaskByIdAsync(taskId, cancellationToken);

        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

        var uploadResult = await UploadFilesFromRequestBodyAsync(taskId, fileStream, contentType, cancellationToken);
        var fileCreationDbResult = _fileRepository.CreateFilesAsync(taskId, uploadResult.files, cancellationToken);

        scope.Complete();

        return uploadResult.uploadResult;
    }

    private static async Task<(FileUploadSummary uploadResult, List<FileModel> files)> UploadFilesFromRequestBodyAsync(Guid taskId, Stream fileStream, string contentType, CancellationToken cancellationToken)
    {
        var fileCount = 0;
        long totalSizeInBytes = 0;

        var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
        var multipartReader = new MultipartReader(boundary, fileStream);
        var section = await multipartReader.ReadNextSectionAsync(cancellationToken);

        var filePaths = new List<string>();
        var fileModels = new List<FileModel>();
        while (section != null)
        {
            var fileSection = section.AsFileSection();
            if (fileSection != null)
            {
                totalSizeInBytes += await SaveFileAsync(taskId, fileSection, filePaths, fileModels, cancellationToken);
                fileCount++;
            }

            section = await multipartReader.ReadNextSectionAsync(cancellationToken);
        }

        return (new FileUploadSummary
        {
            TotalFilesUploaded = fileCount,
            TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
            FilePaths = filePaths,
        }, 
        fileModels);
    }

    private static async Task<long> SaveFileAsync(Guid taskId, FileMultipartSection fileSection, List<string> filePaths, List<FileModel> fileModels, CancellationToken cancellationToken)
    {
        var fileId = Guid.NewGuid();
        var fileIdStr = fileId.ToString();
        var extension = Path.GetExtension(fileSection.FileName);

        Directory.CreateDirectory(UploadsSubDirectory);

        var filePath = Path.Combine(UploadsSubDirectory, taskId.ToString(), $"{fileIdStr}{extension}");

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);

        if (fileSection.FileStream == null)
        {
            throw new UploadFileStreamIsNullException(fileSection.FileName);
        }

        await fileSection.FileStream.CopyToAsync(stream, cancellationToken);

        filePaths.Add(GetFullFilePath(fileIdStr, extension));

        fileModels.Add(new FileModel
        {
            TaskId = taskId,
            FileId = fileId,
            Name = fileSection.FileName,
            ContentType = fileSection.Section.ContentType ?? "undefined",
            Length = fileSection.FileStream.Length,
            UploadDate = DateTime.UtcNow,
        });

        return fileSection.FileStream.Length;
    }

    private static string ConvertSizeToString(long bytes)
    {
        var fileSize = new decimal(bytes);
        var kilobyte = new decimal(1024);
        var megabyte = new decimal(1024 * 1024);
        var gigabyte = new decimal(1024 * 1024 * 1024);

        return fileSize switch
        {
            _ when fileSize < kilobyte => "Less then 1KB",
            _ when fileSize < megabyte =>
                $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
            _ when fileSize < gigabyte =>
                $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
            _ when fileSize >= gigabyte =>
                $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
            _ => "n/a"
        };
    }

    private static string GetBoundary(MediaTypeHeaderValue contentType)
    {
        var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

        if (string.IsNullOrWhiteSpace(boundary))
        {
            throw new InvalidDataException("Missing content-type boundary.");
        }

        return boundary;
    }

    private static string GetFullFilePath(string name, string extension) => Path.Combine(Directory.GetCurrentDirectory(), UploadsSubDirectory, $"{name}{extension}");

    private static string GetTaskFilesPath(Guid taskId) => Path.Combine(Directory.GetCurrentDirectory(), UploadsSubDirectory, taskId.ToString());
}

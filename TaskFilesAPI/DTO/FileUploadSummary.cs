namespace TaskFilesAPI.DTO;

public class FileUploadSummary
{
    public int TotalFilesUploaded { get; set; }
    public string TotalSizeUploaded { get; set; } = null!;
    public List<string> FilePaths { get; set; } = [];
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFilesAPI.Services;
using TaskFilesAPI.Utilities;

namespace TaskFilesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FilesController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("upload-stream-multipartreader")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [MultipartFormData]
        [DisableFormValueModelBinding]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromQuery]Guid taskId, CancellationToken cancellationToken)
        {
            var fileUploadSummary = await _fileService.UploadFilesAsync(taskId, HttpContext.Request.Body, Request.ContentType!, cancellationToken);

            return CreatedAtAction(nameof(Upload), fileUploadSummary);
        }
    }
}

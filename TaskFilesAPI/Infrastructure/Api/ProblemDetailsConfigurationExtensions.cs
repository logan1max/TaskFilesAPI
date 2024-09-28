using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Authentication;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using TaskFilesAPI.Contracts.Exceptions;

namespace TaskFilesAPI.Infrastructure.Api
{
    internal static class ProblemDetailsConfigurationExtensions
    {
        internal const string BadRequestErrorTitle = "Невалидный запрос";
        internal const string ResourceNotFoundErrorTitle = "Ресурс не найден";
        internal const string UnauthorizedErrorTitle = "Нет доступа";
        internal const string UnprocessableEntityErrorTitle = "Операция невозможна";

        internal static IServiceCollection AddProblemDetailsMapping(this IServiceCollection services)
        {
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (ctx, exc) => false;
                options.Map<ResourceNotFoundException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = ResourceNotFoundErrorTitle,
                    Detail = e.Message,
                });
                options.Map<AppException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Title = UnprocessableEntityErrorTitle,
                    Detail = e.Message,
                });
                options.Map<InvalidOperationException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Title = UnprocessableEntityErrorTitle,
                    Detail = e.Message,
                });
                options.Map<KeyNotFoundException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = ResourceNotFoundErrorTitle,
                    Detail = e.Message,
                });
                options.Map<AuthenticationException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.Unauthorized,
                    Title = UnauthorizedErrorTitle,
                    Detail = e.Message
                });
                options.Map<ValidationException>(e => new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = BadRequestErrorTitle,
                    Detail = e.Message,
                });
            });

            return services;
        }
    }
}

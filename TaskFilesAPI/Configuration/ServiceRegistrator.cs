using TaskFilesAPI.Contracts.Interfaces;
using TaskFilesAPI.DataAccess.Repositories;
using TaskFilesAPI.Services;
using TaskFilesAPI.Services.Helpers;

namespace TaskFilesAPI.Configuration;

internal static class ServiceRegistrator
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddScoped<IFileService, FileService>()
            .AddScoped<ITaskService, TaskService>()
            .AddScoped<IFileHelper, FileHelper>();

        return services;
    }

    public static IServiceCollection AddRepositories(this  IServiceCollection services) 
    {
        services
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<IFileRepository, FileRepository>();

        return services;
    }
}

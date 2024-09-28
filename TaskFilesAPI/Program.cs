using System.Reflection;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using TaskFilesAPI.Configuration;
using TaskFilesAPI.DataAccess.Context;
using TaskFilesAPI.DataAccess.Mapping;
using TaskFilesAPI.Infrastructure.Api;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddProblemDetailsMapping();

services
    .AddDbContext<TaskFilesContext>(options =>
    {
        options.UseSqlServer(
                @$"Server=localhost\SQLEXPRESS;Database={DatabaseConstants.TaskFilesDatabaseName};User Id=sa;Password=12345;TrustServerCertificate=True;",
                b => b.MigrationsAssembly(typeof(TaskFilesContext).GetTypeInfo().Assembly.GetName().Name));
    })
    .AddRepositories();

services.AddApplicationServices();

services.AddAutoMapper(config =>
{
    config.AddProfile<DtoMappingProfile>();
    config.AddProfile<EntityMappingProfile>();
});

services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue;
});

services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

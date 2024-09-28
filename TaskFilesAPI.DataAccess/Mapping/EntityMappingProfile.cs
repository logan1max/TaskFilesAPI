using AutoMapper;
using TaskFilesAPI.DataAccess.Context.Entities;

namespace TaskFilesAPI.DataAccess.Mapping;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        CreateMap<Contracts.TaskModel, TaskModel>()
            .ReverseMap();

        CreateMap<Contracts.FileModel, FileModel>()
            .ReverseMap();
    }
}

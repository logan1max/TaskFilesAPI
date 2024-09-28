using AutoMapper;

namespace TaskFilesAPI.Infrastructure.Api;

public class DtoMappingProfile : Profile
{
    public DtoMappingProfile()
    {
        CreateMap<string, DTO.Error>()
            .ForMember(x => x.Message, x => x.MapFrom(e => e));

        CreateMap<Contracts.OperationResult<Guid>, DTO.CreateUpdateResult>()
            .ForMember(x => x.Id, x => x.MapFrom(e => e.Subject));

        CreateMap<Contracts.TaskDeleteOperationResult, DTO.TaskDeleteOperationResult>()
            //.ForMember(x => x.Tasks, x => x.MapFrom(e => e.Tasks))
            //.ForMember(x => x.Files, x => x.MapFrom(e => e.Files))
            //.ForMember(x => x.FolderFiles, x => x.MapFrom(e => e.FolderFiles))
            ;
    }
}

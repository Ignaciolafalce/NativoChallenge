using NativoChallenge.Application.Tasks.DTOs;
using AutoMapper;

namespace NativoChallenge.Application.Tasks.Mapping;

public class TaskDtoProfile : Profile
{
    public TaskDtoProfile()
    {
        CreateMap<Domain.Entities.Task, TaskDto>()
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.State.ToString()));
    }
}

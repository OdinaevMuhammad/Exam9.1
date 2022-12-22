using Domain.Entities;
using Domain.Dtos;
using AutoMapper;
namespace Infrastructure.Mappers
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<AddEmployee, Employee>()
            .ForMember(dest => dest.ProfileImage , opt=> opt.MapFrom(src => src.ProfileImage.FileName)).ReverseMap();
            CreateMap<AddEmployee, GetEmployee>().ReverseMap();
             CreateMap<AddJobHistory, JobHistory>().ReverseMap();
             CreateMap<Job, AddJobDto>().ReverseMap();
             CreateMap<AddJobDto , Job>().ReverseMap();
             CreateMap<AddJobTimeHistory, JobTimeHistory>();
           

        }
    }
}
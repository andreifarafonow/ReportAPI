using AutoMapper;
using ReportAPI.Models.DTO;

namespace ReportAPI.Models.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Report, ReportDto>().ReverseMap();
        }
    }
}

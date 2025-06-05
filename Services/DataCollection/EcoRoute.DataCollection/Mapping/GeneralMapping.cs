using AutoMapper;
using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Dtos.WasteBinDtos;
using EcoRoute.DataCollection.Entities;

namespace EcoRoute.DataCollection.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<Sensor, ResultSensorDto>().ReverseMap();
            CreateMap<Sensor, CreateSensorDto>().ReverseMap();
            CreateMap<Sensor, UpdateSensorDto>().ReverseMap();
            CreateMap<Sensor, GetByIdSensorDto>().ReverseMap();

            CreateMap<WasteBin, ResultWasteBinDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WasteBinId))
                .ReverseMap()
                .ForMember(dest => dest.WasteBinId, opt => opt.MapFrom(src => src.Id));

            CreateMap<WasteBin, CreateWasteBinDto>().ReverseMap();

            CreateMap<WasteBin, UpdateWasteBinDto>().ReverseMap();

            CreateMap<WasteBin, GetByIdWasteBinDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.WasteBinId))
                .ReverseMap()
                .ForMember(dest => dest.WasteBinId, opt => opt.MapFrom(src => src.Id));
        }
    }
}

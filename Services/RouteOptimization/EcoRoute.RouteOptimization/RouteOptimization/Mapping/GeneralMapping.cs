using AutoMapper;
using EcoRoute.RouteOptimization.Dtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<CreateRouteDto, RouteTask>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Scheduled"));

            CreateMap<RouteStep, RouteStepDto>().ReverseMap();
            CreateMap<RouteTask, RouteResultDto>().ReverseMap();

            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, ResultVehicleDto>();

        }
    }
}

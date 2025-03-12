using AutoMapper;
using EcoRoute.RouteOptimization.Dtos.RouteDtos;
using EcoRoute.RouteOptimization.Dtos.RouteOptimizationResultDtos;
using EcoRoute.RouteOptimization.Dtos.WaypointDtos;
using EcoRoute.RouteOptimization.Entities;

namespace EcoRoute.RouteOptimization.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<MyRoute, CreateRouteDto>().ReverseMap();
            CreateMap<MyRoute, UpdateRouteDto>().ReverseMap();
            CreateMap<MyRoute, ResultRouteDto>().ReverseMap();
            CreateMap<MyRoute, GetByIdRouteDto>().ReverseMap();

            CreateMap<RouteOptimizationResult,CreateRouteOptimizationResultDto>().ReverseMap();
            CreateMap<RouteOptimizationResult, UpdateRouteOptimizationResultDto>().ReverseMap();
            CreateMap<RouteOptimizationResult, ResultRouteOptimizationResultDto>().ReverseMap();
            CreateMap<RouteOptimizationResult,GetByIdRouteOptimizationResultDto>().ReverseMap();

            CreateMap<Waypoint,CreateWaypointDto>().ReverseMap();
            CreateMap<Waypoint,UpdateWaypointDto>().ReverseMap();
            CreateMap<Waypoint,GetByIdWaypointDto>().ReverseMap();
            CreateMap<Waypoint,ResultWaypointDto>().ReverseMap();
        }
    }
}

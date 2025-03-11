using AutoMapper;
using EcoRoute.DataCollection.Dtos.BinLogDtos;
using EcoRoute.DataCollection.Dtos.EnvLogDtos;
using EcoRoute.DataCollection.Entities;

namespace EcoRoute.DataCollection.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<BinLog, ResultBinLogDto>().ReverseMap();
            CreateMap<BinLog, CreateBinLogDto>().ReverseMap();
            CreateMap<BinLog, UpdateBinLogDto>().ReverseMap();
            CreateMap<BinLog, GetByIdBinLogDto>().ReverseMap();

            CreateMap<EnvLog, ResultEnvLogDto>().ReverseMap();
            CreateMap<EnvLog, CreateEnvLogDto>().ReverseMap();
            CreateMap<EnvLog, UpdateEnvLogDto>().ReverseMap();
            CreateMap<EnvLog, GetByIdEnvLogDto>().ReverseMap();
        }
    }
}

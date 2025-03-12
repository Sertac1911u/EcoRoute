using AutoMapper;
using EcoRoute.DataCollection.Dtos.BinLogDtos;
using EcoRoute.DataCollection.Dtos.EnvLogDtos;
using EcoRoute.DataCollection.Dtos.ProcessDataDtos;
using EcoRoute.DataCollection.Dtos.SensorDtos;
using EcoRoute.DataCollection.Dtos.WasteBinDtos;
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

            CreateMap<ProcessData, ResultProcessDataDto>().ReverseMap();
            CreateMap<ProcessData, CreateProcessDataDto>().ReverseMap();
            CreateMap<ProcessData, UpdateProcessDataDto>().ReverseMap();
            CreateMap<ProcessData, GetByIdProcessDataDto>().ReverseMap();

            CreateMap<Sensor, ResultSensorDto>().ReverseMap();
            CreateMap<Sensor, CreateSensorDto>().ReverseMap();
            CreateMap<Sensor, UpdateSensorDto>().ReverseMap();
            CreateMap<Sensor, GetByIdSensorDto>().ReverseMap();

            CreateMap<WasteBin, ResultWasteBinDto>().ReverseMap();
            CreateMap<WasteBin, CreateWasteBinDto>().ReverseMap();
            CreateMap<WasteBin, UpdateWasteBinDto>().ReverseMap();
            CreateMap<WasteBin, GetByIdWasteBinDto>().ReverseMap();
        }
    }
}

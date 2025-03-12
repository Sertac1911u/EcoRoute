using AutoMapper;
using EcoRoute.DataProcessing.Dtos.DataProcessingLogDtos;
using EcoRoute.DataProcessing.Dtos.ProcessedDataDtos;
using EcoRoute.DataProcessing.Entities;

namespace EcoRoute.DataProcessing.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            CreateMap<DataProcessingLog, CreateDataProcessingLogDto>().ReverseMap();
            CreateMap<DataProcessingLog, ResultDataProcessingLogDto>().ReverseMap();
            CreateMap<DataProcessingLog, GetByIdDataProcessingLogDto>().ReverseMap();

            CreateMap<ProcessedData, CreateProcessedDataDto>().ReverseMap();
            CreateMap<ProcessedData, UpdateProcessedDataDto>().ReverseMap();
            CreateMap<ProcessedData, ResultProcessedDataDto>().ReverseMap();
            CreateMap<ProcessedData, GetByIdProcessedDataDto>().ReverseMap();
        }
    }
}

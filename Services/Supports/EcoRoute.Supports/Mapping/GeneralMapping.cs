using AutoMapper;
using EcoRoute.Supports.Dtos.SupportTicketDto;
using EcoRoute.Supports.Dtos.TicketResponseDto;
using EcoRoute.Supports.Entities;

namespace EcoRoute.Supports.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping() 
        {
            CreateMap<SupportTicket, CreateSupportTicketDto>().ReverseMap();
            CreateMap<SupportTicket, ResultSupportTicketDto>().ReverseMap();
            CreateMap<SupportTicket, GetByIdSupportTicketDto>().ReverseMap();


            CreateMap<TicketResponse, ResultTicketResponseDto>().ReverseMap();
        }
    }
}

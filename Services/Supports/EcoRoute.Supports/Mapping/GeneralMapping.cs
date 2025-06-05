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
            CreateMap<CreateSupportTicketDto, SupportTicket>();
            CreateMap<SupportTicket, ResultSupportTicketDto>();
            CreateMap<SupportTicket, GetByIdSupportTicketDto>();

            CreateMap<CreateTicketResponseDto, TicketResponse>();
            CreateMap<TicketResponse, ResultTicketResponseDto>();
        }
    }
}

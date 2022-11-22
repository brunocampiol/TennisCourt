using AutoMapper;
using TennisCourt.Application.DTO;
using TennisCourt.Domain.Models;

namespace TennisCourt.Application.AutoMapper
{
    public class DtoToDomainMappingProfile : Profile
    {
        public DtoToDomainMappingProfile()
        {
            CreateMap<ProcessReservationDto, Reservation>()
                .AfterMap((src, dest) =>
                {
                    dest.Id = Guid.NewGuid();
                });

            CreateMap<CancelReservationDto, Reservation>();

            CreateMap<RescheduleReservationDto, Reservation>()
                .ForMember(dest => dest.ReservationDate, opt => opt.MapFrom(src => src.NewReservationDate));
        }
    }
}
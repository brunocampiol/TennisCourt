using AutoMapper;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Interface;
using TennisCourt.Domain.Interfaces.Services;
using TennisCourt.Domain.Models;

namespace TennisCourt.Application.Services
{
    public class ReservationAppService : IReservationAppService
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;

        public ReservationAppService(IReservationService reservationService,
                                     IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
        }

        public async Task<Reservation> CancelReservationAsync(CancelReservationDto dto)
        {
            var entity = _mapper.Map<Reservation>(dto);
            return await _reservationService.CancelReservationAsync(entity);
        }

        public async Task<Reservation> GetReservationAsync(Guid id)
        {
            return await _reservationService.GetReservationAsync(id);
        }

        public async Task<Reservation> ProcessReservationAsync(ProcessReservationDto dto)
        {
            var entity = _mapper.Map<Reservation>(dto);
            return await _reservationService.ProcessReservationAsync(entity);
        }

        public async Task<Reservation> RescheduleReservationAsync(RescheduleReservationDto dto)
        {
            var entity = _mapper.Map<Reservation>(dto);
            return await _reservationService.RescheduleReservationAsync(entity);
        }
    }
}
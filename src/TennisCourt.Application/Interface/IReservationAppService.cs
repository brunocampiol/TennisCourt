using TennisCourt.Application.DTO;
using TennisCourt.Domain.Models;

namespace TennisCourt.Application.Interface
{
    public interface IReservationAppService
    {
        Task<Reservation> ProcessReservationAsync(ProcessReservationDto dto);
        Task<Reservation> CancelReservationAsync(CancelReservationDto dto);
        Task<Reservation> RescheduleReservationAsync(RescheduleReservationDto dto);
        Task<Reservation> GetReservationAsync(Guid id);
    }
}
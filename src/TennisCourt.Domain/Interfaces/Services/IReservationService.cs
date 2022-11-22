using TennisCourt.Domain.Models;

namespace TennisCourt.Domain.Interfaces.Services
{
    public interface IReservationService
    {
        Task<Reservation> ProcessReservationAsync(Reservation entity);
        Task<Reservation> CancelReservationAsync(Reservation reservation);
        Task<Reservation> RescheduleReservationAsync(Reservation reservation);
        Task<Reservation> GetReservationAsync(Guid id);
    }
}
using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Interfaces.Repositories;
using TennisCourt.Domain.Interfaces.Services;
using TennisCourt.Domain.Models;

namespace TennisCourt.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _repository;

        public ReservationService(IReservationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Reservation> CancelReservationAsync(Reservation reservation)
        {
            var currentReservation = await _repository.GetByIdAsync(reservation.Id);

            if (currentReservation != null)
            {
                currentReservation.ReservationDate = null;
                currentReservation.ReservationStatus = ReservationStatusEnum.Cancelled;
                currentReservation.RefundValue = currentReservation.Value;
                currentReservation.Value = 0m;

                await _repository.UpdateAsync(currentReservation);
                return currentReservation;
            }

            return reservation;
        }

        public async Task<Reservation> GetReservationAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Reservation> RescheduleReservationAsync(Reservation reservation)
        {
            var currentReservation = await _repository.GetByIdAsync(reservation.Id);

            if (currentReservation == null)
            {
                return reservation;
            }

            if (!currentReservation.ReservationDate.HasValue)
            {
                currentReservation.ReservationDate = reservation.ReservationDate;
                currentReservation.ReservationStatus = ReservationStatusEnum.Active;

                await _repository.UpdateAsync(currentReservation);
            }

            return currentReservation;
        }

        public async Task<Reservation> ProcessReservationAsync(Reservation entity)
        {
            entity.ReservationStatus = ReservationStatusEnum.Active;
            return await _repository.AddAsync(entity);
        }
    }
}
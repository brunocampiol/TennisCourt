using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TennisCourt.Application.DTO;
using TennisCourt.Application.Interface;
using TennisCourt.Domain.Models;

namespace TennisCourt.Api.Controllers
{
    [Route("reservation")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationAppService _reservationAppService;

        public ReservationController(IReservationAppService reservationService)
        {
            _reservationAppService = reservationService;
        }

        [HttpPost("ProcessReservation")]
        public async Task<Reservation> ProcessReservation(ProcessReservationDto dto)
        {
            return await _reservationAppService.ProcessReservationAsync(dto);
        }

        [HttpPost("CancelReservation")]
        public async Task<Reservation> CancelReservationAsync(CancelReservationDto dto)
        {
            return await _reservationAppService.CancelReservationAsync(dto);
        }

        [HttpGet("GetReservation")]
        public async Task<Reservation> GetReservationAsync([Required]Guid id)
        {
            return await _reservationAppService.GetReservationAsync(id);
        }

        [HttpPost("RescheduleReservation")]
        public async Task<Reservation> RescheduleReservationAsync(RescheduleReservationDto dto)
        {
            return await _reservationAppService.RescheduleReservationAsync(dto);
        }
    }
}
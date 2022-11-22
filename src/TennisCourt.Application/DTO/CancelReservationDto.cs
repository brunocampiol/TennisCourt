using System.ComponentModel.DataAnnotations;

namespace TennisCourt.Application.DTO
{
    public class CancelReservationDto
    {
        [Required]
        public Guid Id { get; set; }
    }
}
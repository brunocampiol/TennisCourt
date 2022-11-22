using System.ComponentModel.DataAnnotations;

namespace TennisCourt.Application.DTO
{
    public class RescheduleReservationDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NewReservationDate { get; set; }
    }
}
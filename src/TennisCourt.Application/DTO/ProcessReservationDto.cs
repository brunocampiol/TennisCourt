using System.ComponentModel.DataAnnotations;

namespace TennisCourt.Application.DTO
{
    public class ProcessReservationDto
    {
        [Required]
        public decimal Value { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime ReservationDate { get; set; }
    }
}
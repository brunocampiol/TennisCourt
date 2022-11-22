using TennisCourt.Domain.Enums;
using TennisCourt.Domain.Models.Base;

namespace TennisCourt.Domain.Models
{
    public class Reservation : BaseEntity
    {
        public decimal Value { get;set; }
        public ReservationStatusEnum ReservationStatus { get; set; }
        public decimal RefundValue { get; set; }
        public DateTime? ReservationDate { get; set; }
    }
}
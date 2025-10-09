using Libray_Managment_System.Enum;

namespace Libray_Managment_System.DTOModels
{
    public class ReservationResponseDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public int BookCopyId { get; set; }
        public DateTime ReservedDate { get; set; }
        public ReservationStatus Status { get; set; }
    }
}

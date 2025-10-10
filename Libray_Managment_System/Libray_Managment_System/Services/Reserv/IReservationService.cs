using Library_Managment_System.DTOModels;
using Libray_Managment_System.Services;

namespace Library_Managment_System.Services.Reserv
{
    public interface IReservationService
    {
        Task<Result<ReservationResponseDto>> ReserveBookAsync(ReservationDto dto);
        Task<Result<string>> ApproveReservationAsync(int reservationId);
        Task<Result<string>> CancelReservationAsync(int reservationId);
        Task<Result<List<ReservationResponseDto>>> GetUserReservationsAsync(int userId);
    }
}

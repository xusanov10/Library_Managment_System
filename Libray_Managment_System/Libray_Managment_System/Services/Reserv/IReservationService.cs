using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Reserv
{
    public interface IReservationService
    {

        Task<ResultDTO<ReservationResponseDto>> ReserveBookAsync(ReservationDto dto);
        Task<ResultDTO<string>> ApproveReservationAsync(int reservationId);
        Task<ResultDTO<string>> CancelReservationAsync(int reservationId);
        Task<ResultDTO<List<ReservationResponseDto>>> GetUserReservationsAsync(int userId);
    }
}

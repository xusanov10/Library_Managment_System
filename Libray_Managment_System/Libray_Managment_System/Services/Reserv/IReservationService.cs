namespace Library_Managment_System.Services.Reserv
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> ReserveBookAsync(ReservationDto dto);
        Task ApproveReservationAsync(int reservationId);
        Task CancelReservationAsync(int reservationId);
        Task<List<ReservationResponseDto>> GetUserReservationsAsync(int userId);
    }
}

using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Borrow
{
    public interface IBorrowService
    {
        Task<BorrowResponseDto> BorrowBookAsync(BorrowDto dto);
        Task<bool> ReturnBookAsync(int borrowId);
        Task<List<BorrowResponseDto>> GetUserBorrowsAsync(int userId);
        Task<List<BorrowResponseDto>> GetOverdueBorrowsAsync();
    }
}

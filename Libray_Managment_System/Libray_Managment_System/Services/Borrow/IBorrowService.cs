using Libray_Managment_System.DTOModels;

namespace Libray_Managment_System.Services.Borrow
{
    public interface IBorrowService
    {
        Task<ResultDTO<BorrowResponseDto>> BorrowBookAsync(BorrowDto dto);
        Task<ResultDTO<bool>> ReturnBookAsync(int borrowId);
        Task<ResultDTO<List<BorrowResponseDto>>> GetUserBorrowsAsync(int userId);
        Task<ResultDTO<List<BorrowResponseDto>>> GetOverdueBorrowsAsync();
    }
}

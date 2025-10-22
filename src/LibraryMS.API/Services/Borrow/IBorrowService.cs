using Library_Managment_System.DTOModels;
using Libray_Managment_System.DtoModels;

namespace Libray_Managment_System.Services.Borrow
{
    public interface IBorrowService
    { 
        Task<Result<BorrowResponseDTO>> BorrowBookAsync(BorrowDTO dto);
        Task<Result<bool>> ReturnBookAsync(int borrowId);
        Task<Result<List<BorrowResponseDTO>>> GetUserBorrowsAsync(int userId);
        Task<Result<List<BorrowResponseDTO>>> GetOverdueBorrowsAsync();
    }
}

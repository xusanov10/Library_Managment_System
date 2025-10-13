using Library_Managment_System.DTOModels;
using Libray_Managment_System.DtoModels;

namespace LibraryMS.Application.Services
{
    public interface IBorrowService
    {
        Task<BorrowResponseDTO> BorrowBookAsync(BorrowDTO dto);
        Task<bool> ReturnBookAsync(int borrowId);
        Task<List<BorrowResponseDTO>> GetUserBorrowsAsync(int userId);
        Task<List<BorrowResponseDTO>> GetOverdueBorrowsAsync();
    }
}

using Library_Managment_System1;
using Library_Managment_System.DTOModels;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Borrow
{
    public class BorrowService : IBorrowService
    {
        private readonly LibraryManagmentSystemContext _context;

        public BorrowService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<BorrowResponseDTO> BorrowBookAsync(BorrowDTO dto)
        {
            var copy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
            if (copy == null || copy.Status != BookCopyStatus.Available)
            {
                throw new Exception("Book copy is not available or already borrowed");
            }

            copy.Status = BookCopyStatus.Borrowed;

            var record = new Borrowrecord
            {
                Userid = dto.UserId,
                Bookcopyid = dto.BookCopyId,
                Borrowdate = DateTime.UtcNow,
                Duedate = DateTime.UtcNow.AddDays(14),
                Status = BorrowStatus.Borrowed
            };

            _context.Borrowrecords.Add(record);
            await _context.SaveChangesAsync();

            return new BorrowResponseDTO
            {
                BorrowRecordId = record.Id,
                UserId = record.Userid,
                BorrowDate = record.Borrowdate.Value,
                DueDate = record.Duedate,
                Status = record.Status
            };
        }

        public async Task<bool> ReturnBookAsync(int borrowId)
        {
            var record = await _context.Borrowrecords.FindAsync(borrowId);
            if (record == null || record.Status != BorrowStatus.Borrowed)
            {
                throw new Exception("Borrow record not found or already returned");
            }

            record.Status = BorrowStatus.Returned;
            record.Returndate = DateTime.UtcNow;

            var copy = await _context.Bookcopies.FindAsync(record.Bookcopyid);

            if (copy != null)
            {
                copy.Status = BookCopyStatus.Available;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<BorrowResponseDTO>> GetUserBorrowsAsync(int userId)
        {
            try
            {
                var records = await _context.Borrowrecords.Where(b => b.Userid == userId).ToListAsync();
                return records.Select(r => new BorrowResponseDTO
                {
                    BorrowRecordId = r.Id,
                    UserId = r.Userid,
                    BookCopyId = r.Bookcopyid,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user borrows: " + ex.Message);
            }
        }

        public async Task<List<BorrowResponseDTO>> GetOverdueBorrowsAsync()
        {
            try
            {
                var records = await _context.Borrowrecords
                    .Where(b => b.Status == BorrowStatus.Borrowed && b.Duedate < DateTime.UtcNow)
                    .ToListAsync();
                return records.Select(r => new BorrowResponseDTO
                {
                    BorrowRecordId = r.Id,
                    UserId = r.Userid,
                    BookCopyId = r.Bookcopyid,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Overdue borrow records olishda xatolik: " + ex.Message);
            }
        }
    }
}


using Libray_Managment_System.DTOModels;
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

        public async Task<ResultDTO<BorrowResponseDto>> BorrowBookAsync(BorrowDto dto)
        {
            var result = new ResultDTO<BorrowResponseDto>();

            var copy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
            if (copy == null)
                return new ResultDTO<BorrowResponseDto>
                {
                    StatusCode = 404,
                    Message = "Book copy not found"
                };

            if (copy.Status != BookCopyStatus.Available)
                return new ResultDTO<BorrowResponseDto>
                {
                    StatusCode = 400,
                    Message = "Book copy is not available or already borrowed"
                };

            copy.Status = BookCopyStatus.Borrowed;

            var record = new Borrowrecord
            {
                UserId = dto.UserId,
                BookcopyId = dto.BookCopyId,
                Borrowdate = DateTime.UtcNow,
                Duedate = dto.Duedate,
                Status = BorrowStatus.Borrowed
            };

            _context.Borrowrecords.Add(record);
            await _context.SaveChangesAsync();

            result.StatusCode = 200;
            result.Message = "Book borrowed successfully";
            result.Data = new BorrowResponseDto
            {
                BorrowRecordId = record.Id,
                UserId = record.UserId,
                BookCopyId = record.BookcopyId,
                BorrowDate = record.Borrowdate.Value,
                DueDate = record.Duedate,
                Status = record.Status
            };

            return result;
        }

        public async Task<ResultDTO<bool>> ReturnBookAsync(int borrowId)
        {
            var result = new ResultDTO<bool>();

            var record = await _context.Borrowrecords.FindAsync(borrowId);
            if (record == null)
                return new ResultDTO<bool> { StatusCode = 404, Message = "Borrow record not found", Data = false };

            if (record.Status != BorrowStatus.Borrowed)
                return new ResultDTO<bool> { StatusCode = 400, Message = "This book has already been returned", Data = false };

            record.Status = BorrowStatus.Returned;
            record.Returndate = DateTime.UtcNow;

            var copy = await _context.Bookcopies.FindAsync(record.BookcopyId);
            if (copy != null)
                copy.Status = BookCopyStatus.Available;

            await _context.SaveChangesAsync();

            result.StatusCode = 200;
            result.Message = "Book returned successfully";
            result.Data = true;
            return result;
        }

        public async Task<ResultDTO<List<BorrowResponseDto>>> GetUserBorrowsAsync(int userId)
        {
            var result = new ResultDTO<List<BorrowResponseDto>>();
            try
            {
                var records = await _context.Borrowrecords
                    .Where(b => b.UserId == userId)
                    .ToListAsync();

                result.StatusCode = 200;
                result.Message = "User borrows fetched successfully";
                result.Data = records.Select(r => new BorrowResponseDto
                {
                    BorrowRecordId = r.Id,
                    UserId = r.UserId,
                    BookCopyId = r.BookcopyId,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Error fetching user borrows: " + ex.Message;
                result.Data = new List<BorrowResponseDto>();
            }
            return result;
        }

        public async Task<ResultDTO<List<BorrowResponseDto>>> GetOverdueBorrowsAsync()
        {
            var result = new ResultDTO<List<BorrowResponseDto>>();
            try
            {
                var records = await _context.Borrowrecords
                    .Where(b => b.Status == BorrowStatus.Borrowed && b.Duedate < DateTime.UtcNow)
                    .ToListAsync();

                result.StatusCode = 200;
                result.Message = "Overdue borrow records fetched successfully";
                result.Data = records.Select(r => new BorrowResponseDto
                {
                    BorrowRecordId = r.Id,
                    UserId = r.UserId,
                    BookCopyId = r.BookcopyId,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Error fetching overdue borrows: " + ex.Message;
                result.Data = new List<BorrowResponseDto>();
            }
            return result;
        }
    }
}

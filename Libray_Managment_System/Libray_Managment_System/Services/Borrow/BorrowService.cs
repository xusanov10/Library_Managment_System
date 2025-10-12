using Library_Managment_System.DTOModels;
using Microsoft.EntityFrameworkCore;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.Borrow;
using Libray_Managment_System.Services;

namespace Library_Managment_System.Services.Borrow
{
    public class BorrowService : IBorrowService
    {
        private readonly LibraryManagmentSystemContext _context;

        public BorrowService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<Result<BorrowResponseDTO>> BorrowBookAsync(BorrowDTO dto)
        {
            try
            {
                var copy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
                if (copy == null || copy.Status != BookCopyStatus.Available)
                {
                    return new Result<BorrowResponseDTO>
                    {
                        Message = "Book copy is not available or already borrowed",
                        StatusCode = 400
                    };
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

                var response = new BorrowResponseDTO
                {
                    BorrowRecordId = record.Id,
                    UserId = record.Userid,
                    BookCopyId = record.Bookcopyid,
                    BorrowDate = record.Borrowdate.Value,
                    DueDate = record.Duedate,
                    Status = record.Status
                };

                return new Result<BorrowResponseDTO>
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<BorrowResponseDTO>
                {
                    Message = $"Error borrowing book: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
        public async Task<Result<bool>> ReturnBookAsync(int borrowId)
        {
            try
            {
                var record = await _context.Borrowrecords.FindAsync(borrowId);
                if (record == null || record.Status != BorrowStatus.Borrowed)
                {
                    return new Result<bool>
                    {
                        Message = "Borrow record not found or already returned",
                        StatusCode = 400
                    };
                }

                record.Status = BorrowStatus.Returned;
                record.Returndate = DateTime.UtcNow;

                var copy = await _context.Bookcopies.FindAsync(record.Bookcopyid);
                if (copy == null)
                {
                    return new Result<bool>
                    {
                        Message = "Book copy not found",
                        StatusCode = 400
                    };
                }

                copy.Status = BookCopyStatus.Available;

                await _context.SaveChangesAsync();
                return new Result<bool>
                {
                    Data = true,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    Message = $"Error returning book: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
        public async Task<Result<List<BorrowResponseDTO>>> GetUserBorrowsAsync(int userId)
        {
            try
            {
                var records = await _context.Borrowrecords
                    .Where(b => b.Userid == userId)
                    .ToListAsync();

                var response = records.Select(r => new BorrowResponseDTO
                {
                    BorrowRecordId = r.Id,
                    UserId = r.Userid,
                    BookCopyId = r.Bookcopyid,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();

                return new Result<List<BorrowResponseDTO>>
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<List<BorrowResponseDTO>>
                {
                    Message = $"Error fetching user borrows: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
        public async Task<Result<List<BorrowResponseDTO>>> GetOverdueBorrowsAsync()
        {
            try
            {
                var records = await _context.Borrowrecords
                    .Where(b => b.Status == BorrowStatus.Borrowed && b.Duedate < DateTime.UtcNow)
                    .ToListAsync();

                var response = records.Select(r => new BorrowResponseDTO
                {
                    BorrowRecordId = r.Id,
                    UserId = r.Userid,
                    BookCopyId = r.Bookcopyid,
                    BorrowDate = r.Borrowdate.Value,
                    DueDate = r.Duedate,
                    Status = r.Status
                }).ToList();

                return new Result<List<BorrowResponseDTO>>
                {
                    Data = response,
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new Result<List<BorrowResponseDTO>>
                {
                    Message = $"Error fetching overdue borrows: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}
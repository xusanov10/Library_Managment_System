using Library_Managment_System.DTOModels;
using Libray_Managment_System.DtoModels;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services;
using Libray_Managment_System.Services.Borrow;
using Microsoft.EntityFrameworkCore;

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
                        StatusCode = 400,
                        Message = "Book copy is not available or already borrowed"
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
                    StatusCode = 200,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new Result<BorrowResponseDTO>
                {
                    StatusCode = 500,
                    Message = $"Error borrowing book: {ex.Message}"
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
                        StatusCode = 400,
                        Message = "Borrow record not found or already returned",
                        Data = false
                    };
                }

                record.Status = BorrowStatus.Returned;
                record.Returndate = DateTime.UtcNow;

                var copy = await _context.Bookcopies.FindAsync(record.Bookcopyid);
                if (copy == null)
                {
                    return new Result<bool>
                    {
                        StatusCode = 400,
                        Message = "Book copy not found",
                        Data = false
                    };
                }

                copy.Status = BookCopyStatus.Available;

                await _context.SaveChangesAsync();
                return new Result<bool>
                {
                    StatusCode = 200,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new Result<bool>
                {
                    StatusCode = 500,
                    Message = $"Error returning book: {ex.Message}",
                    Data = false
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
                    StatusCode = 200,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new Result<List<BorrowResponseDTO>>
                {
                    StatusCode = 500,
                    Message = $"Error fetching user borrows: {ex.Message}"
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
                    StatusCode = 200,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new Result<List<BorrowResponseDTO>>
                {
                    StatusCode = 500,
                    Message = $"Error fetching overdue borrows: {ex.Message}"
                };
            }
        }
    }
}
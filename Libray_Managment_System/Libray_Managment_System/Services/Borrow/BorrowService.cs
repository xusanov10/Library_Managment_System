using Library_Managment_System;
using Library_Managment_System.DTOModels;
using LibraryManagment.Api.DTOModels;
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

        public async Task<Result<BorrowResponseDTO>> BorrowBookAsync(BorrowDTO dto)
        {
            var result = new Result<BorrowResponseDTO>();

            try
            {
                var copy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
                if (copy == null || copy.Status != BookCopyStatus.Available)
                {
                    result.StatusCode = 400;
                    result.Message = "Kitob nusxasi mavjud emas yoki allaqachon band.";
                    return result;
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

                result.StatusCode = 200;
                result.Message = "Kitob muvaffaqiyatli olindi.";
                result.Data = new BorrowResponseDTO
                {
                    BorrowRecordId = record.Id,
                    UserId = record.Userid,
                    BorrowDate = record.Borrowdate.Value,
                    DueDate = record.Duedate,
                    Status = record.Status
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Xatolik: " + ex.Message;
            }

            return result;
        }

        public async Task<Result<bool>> ReturnBookAsync(int borrowId)
        {
            var result = new Result<bool>();

            try
            {
                var record = await _context.Borrowrecords.FindAsync(borrowId);
                if (record == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Borrow yozuvi topilmadi";
                    return result;
                }

                if (record.Status == BorrowStatus.Returned)
                {
                    result.StatusCode = 400;
                    result.Message = "Bu kitob allaqachon qaytarilgan";
                    return result;
                }

                record.Status = BorrowStatus.Returned;
                record.Returndate = DateTime.UtcNow;

                var copy = await _context.Bookcopies.FindAsync(record.Bookcopyid);
                if (copy != null)
                    copy.Status = BookCopyStatus.Available;

                await _context.SaveChangesAsync();

                result.StatusCode = 200;
                result.Message = "Kitob muvaffaqiyatli qaytarildi";
                result.Data = true;
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Xatolik: " + ex.Message;
            }

            return result;
        }

        public async Task<Result<List<UserBorrowsResponseDTO>>> GetUserBorrowsAsync(int userId)
        {
            var result = new Result<List<UserBorrowsResponseDTO>>();

            try
            {
                var records = await _context.Borrowrecords
                    .Include(b => b.Bookcopy)
                    .ThenInclude(c => c.Book)
                    .Where(b => b.Userid == userId)
                    .ToListAsync();

                result.Data = records.Select(r => new UserBorrowsResponseDTO
                {
                    BorrowId = r.Id,
                    BookTitle = r.Bookcopy.Book.Title,
                    BorrowDate = r.Borrowdate ?? DateTime.MinValue,
                    DueDate = r.Duedate,
                    IsReturned = r.Status == BorrowStatus.Returned
                }).ToList();

                result.StatusCode = 200;
                result.Message = "Foydalanuvchi olingan kitoblar ro‘yxati.";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Xatolik: " + ex.Message;
            }

            return result;
        }

        public async Task<Result<List<OverdueBorrowsResponseDTO>>> GetOverdueBorrowsAsync()
        {
            var result = new Result<List<OverdueBorrowsResponseDTO>>();

            try
            {
                var records = await _context.Borrowrecords
                    .Include(b => b.Bookcopy)
                    .ThenInclude(c => c.Book)
                    .Include(b => b.User)
                    .Where(b => b.Status == BorrowStatus.Borrowed && b.Duedate < DateTime.UtcNow)
                    .ToListAsync();

                result.Data = records.Select(r => new OverdueBorrowsResponseDTO
                {
                    BorrowId = r.Id,
                    BookTitle = r.Bookcopy.Book.Title,
                    UserName = r.User.Username,
                    DueDate = r.Duedate,
                    DaysOverdue = (DateTime.UtcNow - r.Duedate).Days,
                    FineAmount = (DateTime.UtcNow - r.Duedate).Days * 0.5m 
                }).ToList();

                result.StatusCode = 200;
                result.Message = "Kechikkan kitoblar royxati";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = "Xatolik: " + ex.Message;
            }

            return result;
        }
    }
}

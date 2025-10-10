using Library_Managment_System;
using Library_Managment_System.DTOModels;
using Microsoft.EntityFrameworkCore;
using Library_Managment_System1;
namespace Libray_Managment_System.Services.Fine
{
    public class FineService
    {
        private readonly LibraryManagmentSystemContext _context;
        public FineService(LibraryManagmentSystemContext context) => _context = context;


        public async Task<int> CalculateFineAsync(int borrowId)
        {
            var borrow = await _context.Borrowrecords.FindAsync(borrowId);

            if (borrow is null) throw new Exception();

            if (borrow.Duedate > DateTime.Now) return 0;

            TimeSpan diff = DateTime.Now - borrow.Duedate;

            return diff.Days * 1000;
        }

        public async Task<FineDTO> CreateFineAsync(int borrowId)
        {
            var fineAmount = await CalculateFineAsync(borrowId);
            if (fineAmount == 0) throw new Exception("Jarima yo'q");

            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            var fine = new Models.Fine
            {
                Userid = borrow.Userid,
                Borrowrecordid = borrowId,
                Amount = fineAmount,
                Paid = false,
                Createdat = DateTime.UtcNow
            };
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();


            return new FineDTO { Id = fine.Id, UserId = fine.Userid, BorrowRecordId = fine.Borrowrecordid, Amount = fine.Amount, Paid = fine.Paid ?? false };
        }

        public async Task<List<FineDTO>> GetUserFinesAsync(int userId)
        {
            try
            {
                var fines = await _context.Fines.Where(f => f.Userid == userId).ToListAsync();
                return fines.Select(f => new FineDTO
                {
                    Id = f.Id,
                    UserId = f.Userid,
                    BorrowRecordId = f.Borrowrecordid,
                    Amount = f.Amount,
                    Paid = f.Paid ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Foydalanuvchi jarimalarini olishda xatolik: " + ex.Message);
            }
        }
    }
}

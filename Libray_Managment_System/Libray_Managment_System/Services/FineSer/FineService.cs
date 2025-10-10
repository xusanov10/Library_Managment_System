using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.Fine;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment_System.Services.FineSer
{
    public class FineService : IFineService
    {
        private readonly LibraryManagmentSystemContext _context;
        public FineService(LibraryManagmentSystemContext context) => _context = context;
        public async Task<int> CalculateFineAsync(int borrowId)
        {
            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            if (borrow is null)
                throw new Exception("Borrow record not found");
            if (borrow.Duedate > DateTime.Now)
                return 0;
            TimeSpan diff = DateTime.Now - borrow.Duedate;
            return diff.Days * 1000;
        }
        public async Task<FineDto> CreateFineAsync(int borrowId)
        {
            var fineAmount = await CalculateFineAsync(borrowId);
            if (fineAmount == 0)
                throw new Exception("No fine");
            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            var fine = new Fine
            {
                UserId = borrow.UserId,
                BorrowRecordId = borrowId,
                Amount = fineAmount,
                Paid = false
            };
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();
            return new FineDto { Id = fine.Id, UserId = fine.UserId, BorrowRecordId = fine.BorrowRecordId, Amount = fine.Amount, Paid = fine.Paid ?? false };
        }
        public async Task<List<FineDto>> GetUserFinesAsync(int userId)
        {
            try
            {
                var fines = await _context.Fines.Where(f => f.UserId == userId).ToListAsync();
                return fines.Select(f => new FineDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    BorrowRecordId = f.BorrowRecordId,
                    Amount = f.Amount,
                    Paid = f.Paid ?? false
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user fines: " + ex.Message);
            }
        }
    }

}
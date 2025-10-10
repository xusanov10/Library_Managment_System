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
        public async Task<ResultDTO<int>> CalculateFineAsync(int borrowId)
        {
            var result = new ResultDTO<int>()
            {
                Message = "Fine calculated successfully",
                StatusCode = 200
            };

            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            if (borrow is null)
                throw new Exception("Borrow record not found");
            if (borrow.Duedate > DateTime.Now)
            {
                result.Data = 0;

                return result;
            }
            TimeSpan diff = DateTime.Now - borrow.Duedate;
            result.Data =  diff.Days * 1000;
            return result;
        }
        public async Task<ResultDTO<FineDto>> CreateFineAsync(int borrowId)
        {
            var result = new ResultDTO<FineDto>()
            {
                Message = "Fine created successfully",
                StatusCode = 200
            };
            var fineAmount = await CalculateFineAsync(borrowId);
            if (fineAmount.Data == 0)
                throw new Exception("No fine");
            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            var fine = new Fine
            {
                UserId = borrow.UserId,
                BorrowRecordId = borrowId,
                Amount = fineAmount.Data,
                Paid = false
            };
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();
            result.Data = new FineDto { Id = fine.Id, UserId = fine.UserId, BorrowRecordId = fine.BorrowRecordId, Amount = fine.Amount, Paid = fine.Paid ?? false };
            return result;
        }
        public async Task<ResultDTO<List<FineDto>>> GetUserFinesAsync(int userId)
        {
            var result = new ResultDTO<List<FineDto>>()
            {
                Message = "User fines fetched successfully",
                StatusCode = 200
            };
            try
            {
                var fines = await _context.Fines.Where(f => f.UserId == userId).ToListAsync();
                 result.Data =  fines.Select(f => new FineDto
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
            return result;
        }
    }

}
using Library_Managment_System;
using Library_Managment_System.DTOModels;
using Microsoft.EntityFrameworkCore;

namespace Library_Managment_System.Services.FineSer
{
    public class FineService : IFineService
    {
        private readonly LibraryManagmentSystemContext _context;

        public FineService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> CalculateFineAsync(int borrowId)
        {
            var result = new Result<int>
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
            result.Data = diff.Days * 1000;
            return result;
        }

        public async Task<Result<FineDTO>> CreateFineAsync(int borrowId)
        {
            var result = new Result<FineDTO>
            {
                Message = "Fine created successfully",
                StatusCode = 200
            };

            var fineAmount = await CalculateFineAsync(borrowId);
            if (fineAmount.Data == 0)
                throw new Exception("No fine");

            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            if (borrow is null)
                throw new Exception("Borrow record not found");

            var fine = new Fine
            {
                Userid = borrow.Userid,
                Borrowrecordid = borrowId,
                Amount = fineAmount.Data,
                Paid = false
            };

            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();

            result.Data = new FineDTO
            {
                Id = fine.Id,
                UserId = fine.Userid,
                BorrowRecordId = fine.Borrowrecordid,
                Amount = fine.Amount,
                Paid = fine.Paid ?? false
            };
            return result;
        }

        public async Task<Result<List<FineDTO>>> GetUserFinesAsync(int userId)
        {
            var result = new Result<List<FineDTO>>
            {
                Message = "User fines fetched successfully",
                StatusCode = 200
            };

            try
            {
                var fines = await _context.Fines
                    .Where(f => f.Userid == userId)
                .ToListAsync();

                result.Data = fines.Select(f => new FineDTO
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
                throw new Exception("Error fetching user fines: " + ex.Message);
            }

            return result;
        }
    }
}

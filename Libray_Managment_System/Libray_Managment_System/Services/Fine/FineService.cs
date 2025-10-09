using Libray_Managment_System.Models;

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

        public async Task<FineDto> CreateFineAsync(int borrowId)
        {
            var fineAmount = await CalculateFineAsync(borrowId);
            if (fineAmount == 0) throw new Exception("Jarima yo'q");

            var borrow = await _context.Borrowrecords.FindAsync(borrowId);
            var fine = new Fine
            {
                UserId = borrow.Userid,
                BorrowrecordId = borrowId,
                Amount = fineAmount,
                Paid = false
            };
            _context.Fines.Add(fine);
            await _context.SaveChangesAsync();


            return new FineDto { Id = fine.Id, UserId = fine.UserId, BorrowRecordId = fine.BorrowrecordId, Amount = fine.Amount, Paid = fine.Paid ?? false };
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
                    BorrowRecordId = f.BorrowrecordId,
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

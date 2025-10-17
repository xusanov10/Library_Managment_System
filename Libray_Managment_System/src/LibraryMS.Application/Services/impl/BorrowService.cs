using Library_Managment_System1;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Libray_Managment_System.Services.Borrow;
using Libray_Managment_System.Services;
using Microsoft.EntityFrameworkCore;
using LibraryMS.Application.Services;
using LibraryMS.Application.Models.Borrow;

public class BorrowService : IBorrowService
{
    private readonly LibraryManagmentSystemContext _context;

    public BorrowService(LibraryManagmentSystemContext context)
    {
        _context = context;
    }

    public async Task<Result<BorrowResponseDTO>> BorrowBookAsync(BorrowDTO dto)
    {
        var result = new Result<BorrowResponseDTO>
        {
            Message = "Succes",
            StatusCode = 200
        };
        var copy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
        if (copy == null)
        {
            result.Data = null;
            result.Message = "Book copy not found";
            result.StatusCode = 404;
            return result;
        }

        if (copy.Status != BookCopyStatus.Available)
        {
            result.Data = null;
            result.Message = "Book copy is not available";
            result.StatusCode = 400;
            return result;
        }

        copy.Status = BookCopyStatus.Borrowed;

        var record = new Borrowrecord
        {
            Userid = dto.UserId,
            Bookcopyid = dto.BookCopyId,
            Borrowdate = DateTime.UtcNow,
            Duedate = dto.Duedate,
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
        result = new Result<BorrowResponseDTO>
        {
            Data = response,
            Message = "Book borrowed successfully",
            StatusCode = 200
        };
        return result;
    }

    public async Task<Result<bool>> ReturnBookAsync(int borrowId)
    {
        var record = await _context.Borrowrecords.FindAsync(borrowId);
        var result = new Result<bool>()
        {
            Message = "Book return failed",
            StatusCode = 400
        };
        if (record == null)
        {
            result.Data = false;
            return result;
        }

        if (record.Status != BorrowStatus.Borrowed)
        {
            result.Data = false;
            return result;
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

    public async Task<Result<List<BorrowResponseDTO>>> GetUserBorrowsAsync(int userId)
    {
        var result = new Result<List<BorrowResponseDTO>>()
        {
            Message = "Success",
            StatusCode = 200
        };
        try
        {
            var records = await _context.Borrowrecords
                .Where(b => b.Userid == userId)
                .ToListAsync();

            result.Data = records.Select(r => new BorrowResponseDTO
            {
                BorrowRecordId = r.Id,
                UserId = r.Userid,
                BookCopyId = r.Bookcopyid,
                BorrowDate = r.Borrowdate.Value,
                DueDate = r.Duedate,
                Status = r.Status
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            result.Data = null;
            result.Message = "Error fetching user borrows: " + ex.Message;
            result.StatusCode = 500;
            return result;
        }
    }

    public async Task<Result<List<BorrowResponseDTO>>> GetOverdueBorrowsAsync()
    {
        var result = new Result<List<BorrowResponseDTO>>()
        {
            Message = "Success",
            StatusCode = 200
        };
        try
        {
            var records = await _context.Borrowrecords
                .Where(b => b.Status == BorrowStatus.Borrowed && b.Duedate < DateTime.UtcNow)
                .ToListAsync();

            result.Data = records.Select(r => new BorrowResponseDTO
            {
                BorrowRecordId = r.Id,
                UserId = r.Userid,
                BookCopyId = r.Bookcopyid,
                BorrowDate = r.Borrowdate.Value,
                DueDate = r.Duedate,
                Status = r.Status
            }).ToList();

            return result;
        }
        catch (Exception ex)
        {
            result.Data = null;
            result.Message = "Error fetching overdue borrows: " + ex.Message;
            result.StatusCode = 500;
        }
        return result;
    }
}

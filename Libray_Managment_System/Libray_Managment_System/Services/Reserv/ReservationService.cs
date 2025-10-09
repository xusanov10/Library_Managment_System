using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Reserv
{
    public class ReservationService : IReservationService
    {
        private readonly LibraryManagmentSystemContext _context;
        public ReservationService(LibraryManagmentSystemContext context)
        {
            _context = context;
        }
        public async Task<ReservationResponseDto> ReserveBookAsync(ReservationDto dto)
        {
            var bookCopy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
            if (bookCopy == null)
            {
                throw new Exception("Book copy not found");
            }
            if (bookCopy.Status == BookCopyStatus.Reserved || bookCopy.Status == BookCopyStatus.Borrowed)
            {
                throw new Exception("Book copy is already reserved");
            }
            var reservation = new Reservation
            {
                UserId = dto.UserId,
                BookcopyId = dto.BookCopyId,
                ReservedDate = DateTime.UtcNow,
                Status = ReservationStatus.Pending
            };
            _context.Reservations.Add(reservation);
            bookCopy.Status = BookCopyStatus.Reserved;
            await _context.SaveChangesAsync();
            return new ReservationResponseDto
            {
                ReservationId = reservation.Id,
                UserId = reservation.UserId,
                BookCopyId = reservation.BookcopyId,
                ReservedDate = reservation.ReservedDate ?? default(DateTime),
                Status = reservation.Status
            };
        }
        public async Task ApproveReservationAsync(int reservationId)
        {
            var res = await _context.Reservations.FindAsync(reservationId);
            if (res == null || res.Status != ReservationStatus.Pending)
                throw new Exception("Reservation already approved or not found");
            res.Status = ReservationStatus.Approved;
            await _context.SaveChangesAsync();
        }
        public async Task CancelReservationAsync(int reservationId)
        {
            var res = await _context.Reservations.FindAsync(reservationId);
            if (res == null)
                throw new Exception("Reservation not found");
            res.Status = ReservationStatus.Cancelled;
            var copy = await _context.Bookcopies.FindAsync(res.BookcopyId);
            if (copy != null)
                copy.Status = BookCopyStatus.Available;
            await _context.SaveChangesAsync();
        }
        public async Task<List<ReservationResponseDto>> GetUserReservationsAsync(int userId)
        {
            try
            {
                var res = await _context.Reservations.Where(r => r.UserId == userId).ToListAsync();
                return res.Select(r => new ReservationResponseDto
                {
                    ReservationId = r.Id,
                    UserId = r.UserId,
                    BookCopyId = r.BookcopyId,
                    ReservedDate = r.ReservedDate ?? default(DateTime),
                    Status = r.Status
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching user reservations: " + ex.Message);
            }
        }
    }
}

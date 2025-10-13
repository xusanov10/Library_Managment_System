using Library_Managment_System.DTOModels;
using Library_Managment_System.Services.Reserv;
using Library_Managment_System;
using Libray_Managment_System.DTOModels;
using Libray_Managment_System.Enum;
using Libray_Managment_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Libray_Managment_System.Services.Reserv
{
    public class ReservationService : IReservationService
    {
        private readonly LibraryManagmentSystemContext _context;
        public ReservationService(LibraryManagmentSystemContext context) => _context = context;

        public async Task<Result<ReservationResponseDto>> ReserveBookAsync(ReservationDto dto)
        {
            var result = new Result<ReservationResponseDto>();

            try
            {
                var bookCopy = await _context.Bookcopies.FindAsync(dto.BookCopyId);
                if (bookCopy == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Book copy not found";
                    return result;
                }

                if (bookCopy.Status == BookCopyStatus.Reserved || bookCopy.Status == BookCopyStatus.Borrowed)
                {
                    result.StatusCode = 400;
                    result.Message = "Book copy is already reserved or borrowed";
                    return result;
                }

                var reservation = new Reservation
                {
                    Userid = dto.UserId,
                    Bookcopyid = dto.BookCopyId,
                    Reserveddate = DateTime.UtcNow,
                    Status = ReservationStatus.Pending
                };

                _context.Reservations.Add(reservation);
                bookCopy.Status = BookCopyStatus.Reserved;
                await _context.SaveChangesAsync();

                result.StatusCode = 200;
                result.Message = "Book reserved successfully";
                result.Data = new ReservationResponseDto
                {
                    ReservationId = reservation.Id,
                    UserId = reservation.Userid,
                    BookCopyId = reservation.Bookcopyid,
                    ReservedDate = reservation.Reserveddate ?? default,
                    Status = reservation.Status
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error reserving book: {ex.Message}";
            }

            return result;
        }

        public async Task<Result<string>> ApproveReservationAsync(int reservationId)
        {
            var result = new Result<string>();

            try
            {
                var reservation = await _context.Reservations.FindAsync(reservationId);
                if (reservation == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Reservation not found";
                    return result;
                }

                if (reservation.Status != ReservationStatus.Pending)
                {
                    result.StatusCode = 400;
                    result.Message = "Reservation already approved or cancelled";
                    return result;
                }

                reservation.Status = ReservationStatus.Approved;
                await _context.SaveChangesAsync();

                result.StatusCode = 200;
                result.Message = "Reservation approved successfully";
                result.Data = "Approved";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error approving reservation: {ex.Message}";
            }

            return result;
        }

        public async Task<Result<string>> CancelReservationAsync(int reservationId)
        {
            var result = new Result<string>();

            try
            {
                var reservation = await _context.Reservations.FindAsync(reservationId);
                if (reservation == null)
                {
                    result.StatusCode = 404;
                    result.Message = "Reservation not found";
                    return result;
                }

                reservation.Status = ReservationStatus.Cancelled;

                var copy = await _context.Bookcopies.FindAsync(reservation.Bookcopyid);
                if (copy != null)
                    copy.Status = BookCopyStatus.Available;

                await _context.SaveChangesAsync();

                result.StatusCode = 200;
                result.Message = "Reservation cancelled successfully";
                result.Data = "Cancelled";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error cancelling reservation: {ex.Message}";
            }

            return result;
        }

        public async Task<Result<List<ReservationResponseDto>>> GetUserReservationsAsync(int userId)
        {
            var result = new Result<List<ReservationResponseDto>>();

            try
            {
                var reservations = await _context.Reservations
                    .Where(r => r.Userid == userId)
                    .ToListAsync();

                result.Data = reservations.Select(r => new ReservationResponseDto
                {
                    ReservationId = r.Id,
                    UserId = r.Userid,
                    BookCopyId = r.Bookcopyid,
                    ReservedDate = r.Reserveddate ?? default,
                    Status = r.Status
                }).ToList();

                result.StatusCode = 200;
                result.Message = "User reservations fetched successfully";
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = $"Error fetching user reservations: {ex.Message}";
            }

            return result;
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OVRM.Data;
using OVRM.DTO;
using OVRM.Models;

namespace OVRM.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        public readonly OVRMDbContext _context;

        public AdminController(OVRMDbContext context)
        {
            _context = context;
        }

        [HttpGet("vehicles")]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetAllVehicles()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            return Ok(vehicles);
        }

        [HttpGet("bookings")]
        public async Task<ActionResult<IEnumerable<BookingResponseDTO>>> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Customer)
                .ToListAsync();

            var bookingResponseDtos = bookings.Select(booking => new BookingResponseDTO
            {
                Id = booking.Id,
                VehicleId = booking.VehicleId,
                CustomerId = booking.CustomerId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                IsDelivery = booking.IsDelivery,
                Status = booking.Status.ToString() // Convert enum to string
            }).ToList();

            return Ok(bookingResponseDtos);
        }

        [HttpGet("customers")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetAllCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }

        [HttpGet("payments")]
        public async Task<ActionResult<IEnumerable<Payment>>> GetAllPayments()
        {
            var payments = await _context.Payments
                .Include(p => p.Booking) // Correctly includes Booking navigation property
                .Include(p => p.Booking.Customer) // Correctly includes Customer through Booking
                .Include(p => p.Booking.Vehicle) // Correctly includes Vehicle through Booking
                .ToListAsync();
            
            var paymentresponseDtos = payments.Select(payment => new PaymentResponseDTO
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                CustomerId = payment.CustomerId,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                Status = payment.Status,
                Currency = payment.Currency,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentDetails = payment.PaymentDetails
            }).ToList();

            return Ok(paymentresponseDtos);
        }

        [HttpPut("bookings/{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int bookingId,[FromQuery] string status)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking == null)
            {
                return NotFound("Booking not found");
            }
            if (!Enum.TryParse<BookingStatus>(status, true, out var parsedStatus))
                return BadRequest("Invalid status. Use Approved or Rejected");

            booking.Status = parsedStatus;
            await _context.SaveChangesAsync();
            return Ok(new {message =$"Booking status updated to {status}"});
        }
    }
}

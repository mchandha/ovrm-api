using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OVRM.Data;
using OVRM.DTO;
using OVRM.Models;

namespace OVRM.Controllers
{
    [Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        public readonly OVRMDbContext _context;
        private readonly ILogger<BookingsController> _logger;
        public BookingsController(OVRMDbContext context, ILogger<BookingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Booking>>> BookVehicle([FromBody] BookingCreateDTO bookingDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found");
                return BadRequest("Customer not found");
            }

            var booking = new Booking
            {
                VehicleId = bookingDto.VehicleId,
                CustomerId = customer.Id, // Corrected to use the customer's ID from the database
                StartDate = bookingDto.StartDate,
                EndDate = bookingDto.EndDate,
                IsDelivery = bookingDto.IsDelivery,
                Status = BookingStatus.Pending
            };

            _logger.LogInformation($"Creating booking for vehicle ID: {bookingDto.VehicleId} by customer ID: {customer.Id}");
            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Booking created successfully");

            // Mapping the Booking entity to BookingResponseDTO
            var bookingResponse = new BookingResponseDTO
            {
                Id = booking.Id,
                VehicleId = booking.VehicleId,
                CustomerId = booking.CustomerId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                IsDelivery = booking.IsDelivery,
                Status = booking.Status.ToString() // Optional: convert enum to string
            };

            return Ok(bookingResponse);
        }

        [HttpGet("mybookings")]
        public async Task<IActionResult> GetMyBookings()
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) 
            {
                _logger.LogError("Customer not found");
                return BadRequest("Customer not found");
            }

            var bookings = await _context.Bookings
                .Include(b => b.Vehicle)
                .Where(b => b.CustomerId == customer.Id)
                .ToListAsync();

            _logger.LogInformation($"Found {bookings.Count} bookings for customer ID: {customer.Id}");
            var bookingDto =bookings.Select(b=> new BookingResponseDTO
            {
                Id = b.Id,
                 VehicleId = b.VehicleId,
                 CustomerId = b.CustomerId,
                 StartDate = b.StartDate,
                 EndDate= b.EndDate,
                IsDelivery = b.IsDelivery,
                Status = b.Status.ToString() // Optional: convert enum to string

            }).ToList();

            return Ok(bookingDto);
        }

        [HttpPost("payment")]
        public async Task<IActionResult> MakePayment([FromBody] PaymentCreateDTO paymentDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
            if (customer == null) return BadRequest("Customer not found");

            var payment = new Payment
            {
                BookingId = paymentDto.BookingId,
                CustomerId = customer.Id,
                PaymentMethod = paymentDto.PaymentMethod,
                TransactionId = paymentDto.TransactionId,
                Status = paymentDto.Status,
                Currency = paymentDto.Currency,
                Amount = paymentDto.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentDetails= paymentDto.PaymentDetails
            };

            _logger.LogInformation($"Processing payment for booking ID: {paymentDto.BookingId} by customer ID: {customer.Id}");
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Payment processed successfully, waiting for approval");

            var paymentresponseDto=new PaymentResponseDTO
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
                PaymentDetails=payment.PaymentDetails
            };
            return Ok(paymentresponseDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Vehicle)
                .Include(b => b.Customer)
                .Include(b => b.Payments) // Corrected from 'Payment' to 'Payments'
                .FirstOrDefaultAsync(b => b.Id == id);
            if (booking == null) return NotFound("Booking not found");

            var bookingDto = new BookingResponseDTO
            {
                Id = booking.Id,
                VehicleId = booking.VehicleId,
                CustomerId = booking.CustomerId,
                StartDate = booking.StartDate,
                EndDate = booking.EndDate,
                IsDelivery = booking.IsDelivery,
                Status = booking.Status.ToString() // Optional: convert enum to string

            };

            return Ok(bookingDto);
        }
    }
}

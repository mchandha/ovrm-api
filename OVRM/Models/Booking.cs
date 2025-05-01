namespace OVRM.Models
{
    public enum BookingStatus
    {
        Pending,
        Approved,
        Rejected
    }
    public class Booking
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDelivery { get; set; }
        public BookingStatus Status { get; set; }

        // Fix: Add the missing Payments collection property  
        public ICollection<Payment> Payments { get; set; }
    }
}

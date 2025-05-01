namespace OVRM.DTO
{
    public class BookingResponseDTO
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDelivery { get; set; }
        public string Status { get; set; } // Use enum's string representation
    }
}

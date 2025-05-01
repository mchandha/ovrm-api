namespace OVRM.DTO
{
    public class BookingCreateDTO
    {
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsDelivery { get; set; }
    }
}

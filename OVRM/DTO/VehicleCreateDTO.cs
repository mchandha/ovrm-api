namespace OVRM.DTO
{
    public class VehicleCreateDTO
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal RentPerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}

namespace OVRM.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public decimal RentPerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}

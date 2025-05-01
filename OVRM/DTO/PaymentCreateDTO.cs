namespace OVRM.DTO
{
    public class PaymentCreateDTO
    {
        public int BookingId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; } // Unique identifier for the transaction
        public string Status { get; set; } // e.g., Completed, Pending, Failed
        public string Currency { get; set; } // e.g., USD, EUR
        public decimal Amount { get; set; } // Amount paid
        public DateTime PaymentDate { get; set; }
        public string PaymentDetails { get; set; } // Additional details about the payment
    }
}

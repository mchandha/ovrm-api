namespace OVRM.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
        public int CustomerId { get; set; } // Foreign key to the Customer table
        public Customer Customer { get; set; } // Reference to the customer who made the payment
        public string PaymentMethod { get; set; } // e.g., Credit Card, PayPal
        public string TransactionId { get; set; } // Unique identifier for the transaction
        public string Status { get; set; } // e.g., Completed, Pending, Failed
        public string Currency { get; set; } // e.g., USD, EUR
        public string PaymentDetails { get; set; } // Additional details about the payment

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}

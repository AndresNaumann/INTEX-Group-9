using Microsoft.Data.SqlClient.DataClassification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickwellStore.Data
{
    public class Order
    {
        [Key]
        public int TransactionId { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public DateOnly Date { get; set; }
        public string DayOfWeek { get; set; }
        public TimeOnly Time { get; set; }
        public string EntryMode { get; set; }
        public int Amount { get; set; }
        public string TransactionType { get; set; }
        public string TransactionCountry { get; set; }
        public string ShippingAddress { get; set; }
        public string Bank { get; set; }
        public string CardType { get; set; }
        public bool Fraud { get; set; }
    }
}

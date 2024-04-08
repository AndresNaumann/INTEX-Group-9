using Microsoft.Data.SqlClient.DataClassification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickwellStore.Models
{
    public class Orders
    {
        [Key]
        public int TransactionId { get; set; }
        [ForeignKey]
        public int CustomerId { get; set; }
        public DateOnly Date {  get; set; }
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

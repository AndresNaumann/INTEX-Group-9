using System.ComponentModel.DataAnnotations.Schema;

namespace BrickwellStore.Models
{
    public class LineItems
    {
        [ForeignKey]
        public int TransactionId { get; set; }
        [ForeignKey]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int Rating { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickwellStore.Data
{
    public class LineItem
    {
        [Key]
        public int LineId { get; set; }  
        public int TransactionId { get; set; }

        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int Rating { get; set; }
    }
}

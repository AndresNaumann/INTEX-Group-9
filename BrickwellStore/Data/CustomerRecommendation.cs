using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BrickwellStore.Data
{
    public class CustomerRecommendation
    {
        [Key]
        public int RecId { get; set; }
        public int CustomerId { get; set; }
        public int RecommendedProductId { get; set; }
    }
}
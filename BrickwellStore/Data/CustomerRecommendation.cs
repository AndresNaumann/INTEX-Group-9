using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace BrickwellStore.Data
{
    public class CustomerRecommendation
    {
        [Key]
        public int customerId { get; set; }
        public int recommendedProductId { get; set; }
    }
}

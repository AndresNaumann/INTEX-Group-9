using System.ComponentModel.DataAnnotations;

namespace BrickwellStore.Data
{
    public class ProductRecommendation
    {
        [Key]
        public int ProductID { get; set; }
        public int RecommendedProductID { get; set; }

    }
}

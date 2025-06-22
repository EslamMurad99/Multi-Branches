using System.ComponentModel.DataAnnotations;

namespace MultiBranches.Models
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<BranchProductModel> BranchProducts { get; set; }
    }
}

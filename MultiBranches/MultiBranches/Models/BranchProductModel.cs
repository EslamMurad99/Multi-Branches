namespace MultiBranches.Models
{
    public class BranchProductModel
    {
        public int BranchId { get; set; } // FK
        public BranchModel Branch { get; set; }

        public int ProductId { get; set; } // Fk
        public ProductModel Product { get; set; }

        public int Quantity { get; set; }
    }
}

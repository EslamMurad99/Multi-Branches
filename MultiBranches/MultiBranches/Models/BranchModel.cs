using System;
using System.ComponentModel.DataAnnotations;

namespace MultiBranches.Models
{
    public class BranchModel
    {
        [Key]
        public int BranchId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public ICollection<BranchProductModel> BranchProducts { get; set; }
    }
}

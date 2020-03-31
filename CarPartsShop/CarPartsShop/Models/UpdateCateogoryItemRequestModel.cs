using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsShop.Models
{
    public class UpdateCategoryItemRequestModel
    {
        public string Name { get; set; }

        public double Price { get; set; }
        [StringLength(256, ErrorMessage = "Description is too long")]
        public string Description { get; set; }

        public Guid ParentCategoryId { get; set; }
        public Guid ItemId { get; set; }
    }
}

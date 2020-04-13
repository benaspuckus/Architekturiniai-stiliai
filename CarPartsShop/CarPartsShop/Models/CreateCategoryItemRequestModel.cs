using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsShop.Models
{
    public class CreateCategoryItemRequestModel
    {
        [StringLength(256, ErrorMessage = "Description is too long")]
        public string Description { get; set; }

        public Guid ParentCategoryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public double Price { get; set; }

        public string ImageData { get; set; }

        public string OemNumber { get; set; }
        public string PartNumber { get; set; }
    }
}

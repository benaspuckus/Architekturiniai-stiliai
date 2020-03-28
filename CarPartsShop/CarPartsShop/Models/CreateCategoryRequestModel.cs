using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CarPartsShop.Models
{
    public class CreateCategoryRequestModel
    {
        [StringLength(256, ErrorMessage = "Description is too long")]
        public string Description { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsShop.Models
{
    public class RemoveCategoryItemRequestModel
    {
        public Guid  CategoryId { get; set; }
        public Guid ItemId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsShop.Models
{
    public class SingleItemResponseModel
    {
        public Guid ItemId { get;  set; }
        public string Name { get;  set; }
        public double Price { get;  set; }
        public string OemNumber { get;  set; }
    }
}

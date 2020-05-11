using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace CarPartsShop.Models
{
    public class ConfirmShoppingCartRequestModel
    {
        public List<ItemModel> Items { get; set; }
        public bool NeedsDelivery { get; set; }
        public string DeliveryAddress { get; set; }

        public double Price => Items.Sum(x => x.Price);
    }

    public class ItemModel
    {
        public Guid ItemId { get; set; }
        public Guid ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string OemNumber { get; set; }

        public string PartNumber { get; set; }
        public string ImageData { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Item
    {
        protected Item()
        {
        }

        private Item(Guid parentId, string name, string description, double price, string image, string oemNumber, string partNumber)
        {
            ItemId = Guid.NewGuid();
            ParentCategoryId = parentId;
            Name = name;
            Description = description;
            Price = price;
            ImageData = image;
            OemNumber = oemNumber;
            PartNumber = partNumber;
        }

        public static Item GetItem(Guid parentId, string name, string description, double price, string image, string oemNumber, string partNumber)
        {
            return new Item(parentId, name, description, price, image, oemNumber, partNumber);
        }

        public Guid ItemId { get; private set; }
        public Guid ParentCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public string OemNumber { get; private set; }

        public string PartNumber { get; private set; }
        public string ImageData { get; private set; }

        public IList<CartItems> CartItems { get; set; }

        public void UpdateItem(string name, string description, double price, string partNumber, string oemNumber)
        {
            Name = string.IsNullOrEmpty(name) ? Name : name;
            Description = string.IsNullOrEmpty(description) ? Description : description;
            Price = price != 0 ? price : Price;
            PartNumber = string.IsNullOrEmpty(partNumber) ? PartNumber : partNumber;
            OemNumber = string.IsNullOrEmpty(oemNumber) ? OemNumber : oemNumber;
        }
    }
}

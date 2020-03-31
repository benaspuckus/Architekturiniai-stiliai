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

        private Item(Guid parentId, string name, string description, double price, string image)
        {
            ItemId = Guid.NewGuid();
            ParentCategoryId = parentId;
            Name = name;
            Description = description;
            Price = price;
            ImageData = image;
        }

        public static Item GetItem(Guid parentId, string name, string description, double price, string image)
        {
            return new Item(parentId, name, description, price, image);
        }

        public Guid ItemId { get; private set; }
        public Guid ParentCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public string ImageData { get; private set; }

        public void UpdateItem(string name, string description, double price)
        {
            Name = string.IsNullOrEmpty(name) ? Name : name;
            Description = string.IsNullOrEmpty(description) ? Description : description;
            Price = price != 0 ? price : Price;
        }
    }
}

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

        public Item(Guid parentId, string name, string description, double price)
        {
            ItemId = Guid.NewGuid();
            ParentCategoryId = parentId;
            Name = name;
            Description = description;
            Price = price;
        }

        public Guid ItemId { get; private set; }
        public Guid ParentCategoryId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
    }
}

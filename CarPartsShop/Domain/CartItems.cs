using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CartItems
    {
        protected CartItems()
        {
        }

        public CartItems(Item item, ShoppingCart shoppingCart)
        {
            Item = item;
            ShoppingCart = shoppingCart;
        }

        public Guid CartItemsId { get; set; }
        public Guid ItemId { get; set; }
        public Guid CartId { get; set; }
        public Item Item { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}

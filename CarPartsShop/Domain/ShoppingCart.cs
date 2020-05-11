using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ShoppingCart
    {
        protected ShoppingCart()
        {
        }
        public Guid CartId { get; private set; }
        public string UserId { get; private set; }
        public CartStatus Status { get; private set; }
        public bool NeedsDelivery { get; private set; }
        public string DeliveryAddress { get; private set; }

        public double Price { get; private set; }
        public IList<CartItems> CartItems { get; set; } = new List<CartItems>();

        public ShoppingCart(string userId, CartStatus status, bool needsDelivery, string deliveryAddress, double price)
        {
            CartId = Guid.NewGuid();
            UserId = userId;
            Status = status;
            NeedsDelivery = needsDelivery;
            DeliveryAddress = deliveryAddress;
            Price = price;
        }

        public ShoppingCart(string userId, CartStatus status, bool needsDelivery, string deliveryAddress, IList<CartItems> cartItems)
        {
            CartId = Guid.NewGuid();
            UserId = userId;
            Status = status;
            NeedsDelivery = needsDelivery;
            DeliveryAddress = deliveryAddress;
            CartItems = cartItems;
        }

        public void AddCartItem(CartItems item)
        {
            CartItems.Add(item);
        }

        public void UpdateStatus(CartStatus status)
        {
            Status = status;
        }
    }
}

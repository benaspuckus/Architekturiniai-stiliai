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
        public IList<CartItems> CartItems { get; set; }

        public ShoppingCart(Guid cartId, string userId, CartStatus status, bool needsDelivery, string deliveryAddress, IList<CartItems> items)
        {
            CartId = cartId;
            UserId = userId;
            Status = status;
            NeedsDelivery = needsDelivery;
            DeliveryAddress = deliveryAddress;
            CartItems = items;
        }

        public void UpdateStatus(CartStatus status)
        {
            Status = status;
        }
    }
}

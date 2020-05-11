using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace CarPartsShop.Models
{
    public class UsersOrdersResponseModel
    {
        public Guid CartId { get; set; }
        public CartStatus Status { get; set; }
        public string DeliveryAddress { get; set; }
        public double Price { get; set; }

        public List<SingleItemResponseModel> Items { get; set;}

    }
}

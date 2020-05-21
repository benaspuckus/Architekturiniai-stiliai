using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPartsShop.Models
{
    public class AllUsersOrdersResponseModel
    {
        public List<UsersOrdersResponseModel> RequestedOrders { get; set; }
        public List<UsersOrdersResponseModel> AcceptedOrders { get; set; }
        public List<UsersOrdersResponseModel> FinishedOrders { get; set; }

    }
}

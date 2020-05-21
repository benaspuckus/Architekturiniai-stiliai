using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;

namespace CarPartsShop.Models
{
    public class ChangeStatusRequestModel
    {
        public Guid CartId { get; set; }
        public CartStatus Status { get; set; }
    }
}

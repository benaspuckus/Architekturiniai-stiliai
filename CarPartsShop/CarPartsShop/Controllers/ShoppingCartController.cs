using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    public class ShoppingCartController : ControllerBase
    {
        public ShoppingCartController()
        {
        }

        [Authorize(Roles = "User")]
        [HttpGet("api/Cart")]
        public IActionResult GetShoppingCart()
        {
            return Ok();
        }
    }
}

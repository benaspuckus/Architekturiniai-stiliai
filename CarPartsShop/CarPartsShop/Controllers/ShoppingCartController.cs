using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        [Authorize(Roles = "User")]
        [HttpGet("api/Cart")]
        public async Task<IActionResult> GetShoppingCart()
        {
            var current = User;
            var userId = current.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserIdNotFound");
            }

            var orders = await _shoppingCartRepository.GetOrdersByUserId(userId);

            return Ok(orders);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CarPartsShop.Models;
using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarPartsShop.Controllers
{
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IItemReadRepository _itemReadRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IItemReadRepository itemReadRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _itemReadRepository = itemReadRepository;
        }

        [Authorize(Roles = "User")]
        [HttpGet("api/Cart/Orders")]
        public async Task<IActionResult> GetShoppingCart()
        {
            try
            {
                var current = User;
                var userId = current.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserIdNotFound");
                }

                var orders = await _shoppingCartRepository.GetOrdersByUserId(userId);

                var responseModel = orders.Select(x => new UsersOrdersResponseModel
                {
                    CartId = x.CartId,
                    Status = x.Status,
                    DeliveryAddress = x.DeliveryAddress,
                    Price = x.Price,
                    Items = x.CartItems.Select(xx => new SingleItemResponseModel
                    {
                        ItemId = xx.Item.ItemId,
                        Name = xx.Item.Name,
                        Price = xx.Item.Price,
                        OemNumber = xx.Item.OemNumber
                    }).ToList()
                }).ToList();

                return Ok(responseModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "User")]
        [HttpPost("api/Cart/Confirm")]
        public async Task<IActionResult> ConfirmShoppingCart([FromBody]ConfirmShoppingCartRequestModel model)
        {
            try
            {
                var current = User;
                var userId = current.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("UserIdNotFound");
                }

                var itemList = new List<Item>();

                foreach (var item in model.Items)
                {
                    var singleItem = await _itemReadRepository.GetSingleItem(item.ItemId);

                    if (singleItem == null)
                    {
                        return BadRequest("Item to purchase is not found");
                    }

                    itemList.Add(singleItem);
                }

                var shoppingCart = new ShoppingCart(userId, CartStatus.Requested, model.NeedsDelivery, model.DeliveryAddress, model.Price);

                foreach (var singleCartItem in itemList.Select(item => new CartItems(item, shoppingCart)))
                {
                    shoppingCart.AddCartItem(singleCartItem);
                }

                _shoppingCartRepository.SaveOrder(shoppingCart);
                _shoppingCartRepository.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

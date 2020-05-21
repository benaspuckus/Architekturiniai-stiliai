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

        [Authorize(Roles = "Admin")]
        [HttpGet("api/Cart/AdminOrders")]
        public async Task<IActionResult> GetAdminOrders()
        {
            try
            {

                var orders = await _shoppingCartRepository.GetAllOrders();

                var requestedOrders = orders.Where(x => x.Status == CartStatus.Requested).Select(x => new UsersOrdersResponseModel
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

                var acceptedOrders = orders.Where(x => x.Status == CartStatus.Accepted).Select(x => new UsersOrdersResponseModel
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

                var finishedOrders = orders.Where(x => x.Status == CartStatus.OrderReady).Select(x => new UsersOrdersResponseModel
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

                var model = new AllUsersOrdersResponseModel
                {
                    RequestedOrders = requestedOrders,
                    AcceptedOrders = acceptedOrders,
                    FinishedOrders = finishedOrders
                };
                

                return Ok(model);
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

                if (model.NeedsDelivery && string.IsNullOrEmpty(model.DeliveryAddress))
                {
                    return BadRequest("No delivery address");
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

                return Ok(shoppingCart.CartId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("api/Cart/ChangeStatus")]
        public IActionResult ChangeStatus([FromBody]ChangeStatusRequestModel model)
        {
            try
            {
                var cart = _shoppingCartRepository.GetOrdersByCartId(model.CartId);
                
                if (cart == null)
                {
                    return BadRequest("Item to purchase is not found");
                }

                cart.UpdateStatus(model.Status);

                _shoppingCartRepository.UpdateOrder(cart);
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

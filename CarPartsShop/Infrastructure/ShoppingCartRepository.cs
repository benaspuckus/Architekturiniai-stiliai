using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetOrdersByCartId(Guid cartId);
        Task<List<ShoppingCart>> GetOrdersByUserId(string userId);
        Task<List<ShoppingCart>> GetAllOrders();
        ShoppingCart UpdateOrder(ShoppingCart order);
        ShoppingCart SaveOrder(ShoppingCart order);
        void SaveChanges();
    }

    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ShopContext _context;

        public ShoppingCartRepository(ShopContext context)
        {
            _context = context;
        }

        public ShoppingCart GetOrdersByCartId(Guid cartId)
        {
            var order = _context
                .ShoppingCarts
                .SingleOrDefault(x => x.CartId == cartId);

            return order;
        }

        public async Task<List<ShoppingCart>> GetOrdersByUserId(string userId)
        {
            var orders = await _context
                .ShoppingCarts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Item)
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return orders;
        }

        public async Task<List<ShoppingCart>> GetAllOrders()
        {
            var orders = await _context
                .ShoppingCarts
                .Include(x => x.CartItems)
                .ThenInclude(x => x.Item)
                .ToListAsync();

            return orders;
        }

        public ShoppingCart UpdateOrder(ShoppingCart order)
        {
            return _context.ShoppingCarts.Update(order).Entity;
        }

        public ShoppingCart SaveOrder(ShoppingCart order)
        {
            return _context.Add(order).Entity;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
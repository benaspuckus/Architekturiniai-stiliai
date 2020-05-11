using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public interface IItemReadRepository
    {
        Task<Item> GetSingleItem(Guid itemId);
    }


    public class ItemReadRepository : IItemReadRepository
    {
        private readonly ShopContext _context;

        public ItemReadRepository(ShopContext context)
        {
            _context = context;
        }

        public async Task<Item> GetSingleItem(Guid itemId)
        {
            var item = await _context
                .Items
                .FirstOrDefaultAsync(x => x.ItemId == itemId);

            return item;
        }
    }
}

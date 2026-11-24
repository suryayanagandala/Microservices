using Cart.ApplicationLayer.Interfaces;
using Cart.CoreModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.Infrastructure.Respositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _db;

        public CartRepository(CartDbContext db)
        {
            _db = db;
        }

        public async Task<Carts?> GetCartByUserId(
            string userId,
            CancellationToken cancellationToken)
        {
            return await _db.Cart.Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive,cancellationToken);
        }

        public async Task CreateCart(Carts cart,CancellationToken cancellationToken)
        {
            await _db.Cart.AddAsync(cart,cancellationToken);
        }

        public async Task AddCartItem(CartItem cartItem,CancellationToken cancellationToken)
        {
            await _db.CartItems.AddAsync(cartItem,cancellationToken);
        }

        public async Task<CartItem?> GetCartItem(int cartId,int productId,CancellationToken cancellationToken)
        {
            return await _db.CartItems.FirstOrDefaultAsync(x => x.CartId == cartId &&
                         x.ProductId == productId &&x.IsActive,cancellationToken);
        }

        public void UpdateCartItem(CartItem cartItem)
        {
            cartItem.IsActive = false;
            cartItem.UpdatedAt = DateTimeOffset.UtcNow;

            _db.CartItems.Update(cartItem);
        }

        public void RemoveCartItem(CartItem cartItem)
        {
            cartItem.IsActive = false;
            cartItem.UpdatedAt = DateTimeOffset.UtcNow;

            _db.CartItems.Update(cartItem);
        }

        public async Task<int> GetCartCount(string userId)
        {
            return await _db.Cart.Where(x => x.UserId == userId && x.IsActive)
                 .SelectMany(x => x.CartItems).Where(x => x.IsActive).SumAsync(ci => ci.Quantity);
        }

        public async Task<Carts?> GetCartWithItems(string userId)
        {
            return await _db.Cart.Include(x => x.CartItems.Where(x => x.IsActive))
                .AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive);
        }

        public async Task<CartItem?> GetCartItemById(int cartItemId, CancellationToken cancellationToken)
        {
            return await _db.CartItems.Include(x=>x.Cart).Where(x => x.CartItemId == cartItemId && x.IsActive).FirstOrDefaultAsync(cancellationToken);
        }

        public void UpdateCartItems(IEnumerable<CartItem> cartItems)
        {
            _db.CartItems.UpdateRange(cartItems);
        }
    }
}

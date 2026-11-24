using Cart.CoreModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cart.ApplicationLayer.Interfaces
{
    public interface ICartRepository
    {
        Task<Carts?> GetCartByUserId(string userId,CancellationToken cancellationToken);

        Task CreateCart(Carts cart,CancellationToken cancellationToken);

        Task AddCartItem(CartItem cartItem,CancellationToken cancellationToken);

        Task<CartItem?> GetCartItem(int cartId,int productId,CancellationToken cancellationToken);

        Task<CartItem?> GetCartItemById(int cartItemId, CancellationToken cancellationToken);

        void UpdateCartItem(CartItem cartItem);

        void RemoveCartItem(CartItem cartItem);


        Task<int> GetCartCount(string userId);

        Task<Carts?> GetCartWithItems(string userId);

        void UpdateCartItems(IEnumerable<CartItem> cartItems);
    }
}

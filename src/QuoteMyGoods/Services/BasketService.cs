using System.Collections.Generic;
using QuoteMyGoods.Models;
using QuoteMyGoods.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace QuoteMyGoods.Services
{
    public class BasketService : IBasketService
    {
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<QMGUser> _userManager;
        private Basket _basket;

        public BasketService(IHttpContextAccessor context, UserManager<QMGUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _basket = new Basket(_context,_userManager);
        }

        public void AddQuantity(int id)
        {
            _basket.GetItemById(id).AddItem();
            _basket.SaveToSession();
        }

        public void AddToBasket(Product product)
        {
            _basket.AddToBasket(product);
        }

        public void ClearBasket()
        {
            _basket.ClearBasket();
        }

        public IDictionary<int, BasketItem> GetBasket()
        {
            return _basket.GetBasket();
        }

        public int GetBasketCount()
        {
            return _basket.GetBasketCount();
        }

        public decimal GetTotalPrice()
        {
            return _basket.GetTotalPrice();
        }

        public void MinusQuantity(int id)
        {
            var res = _basket.GetItemById(id).RemoveItem();
            if(res == 0)
            {
                _basket.RemoveFromBasket(id);
            }
            _basket.SaveToSession();
        }

        public void RemoveFromBasket(int id)
        {
            _basket.RemoveFromBasket(id);
        }

        public void SetBasket(Dictionary<int, BasketItem> basket)
        {
            _basket.SetBasket(basket);
        }
    }
}

using QuoteMyGoods.Common;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Services
{
    public interface IBasketService
    {
        void AddToBasket(Product product);
        void RemoveFromBasket(int id);
        int GetBasketCount();
        IDictionary<int,BasketItem> GetBasket();
        void AddQuantity(int id);
        void MinusQuantity(int id);
        decimal GetTotalPrice();
        void ClearBasket();

        void SetBasket(Dictionary<int, BasketItem> basket);
    }
}

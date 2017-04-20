using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace QuoteMyGoods.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Basket
    {
        private readonly IHttpContextAccessor _context;
        [JsonProperty]
        private Dictionary<int,BasketItem> _Basket { get; set; }
        [JsonProperty]
        private int BasketCount { get; set; }

        private string UserId { get; set; }

        private UserManager<QMGUser> _userManager;

        public Basket(){}

        public Basket(IHttpContextAccessor context, UserManager<QMGUser> userManager)
        {
            _context = context;
            _userManager = userManager;

            UserId = _userManager.GetUserId(_context.HttpContext.User);

            try {
                var basket = _context.HttpContext.Session.GetObjectFromJson<Basket>(UserId);
                if (basket == null)
                {
                    _Basket = new Dictionary<int, BasketItem>();
                    BasketCount = 0;
                    _context.HttpContext.Session.SetObjectAsJson(UserId, this);
                }
                else
                {
                    _Basket = basket._Basket;
                    BasketCount = basket.GetBasketCount();
                }
            }catch(Exception ex)
            {
                _Basket = new Dictionary<int, BasketItem>();
                BasketCount = 0;
                Debug.WriteLine("context was null",ex);
            }
            
        }

        public void ClearBasket()
        {
            _Basket = new Dictionary<int, BasketItem>();
            BasketCount = 0;
            _context.HttpContext.Session.SetObjectAsJson(UserId, this);
            SaveToSession();
        }

        public Basket(int basketCount, Dictionary<int, BasketItem> basket)
        {
            _Basket = basket;
            BasketCount = basketCount;
        }

        public decimal GetTotalPrice()
        {
            return _Basket.Sum(b => b.Value.GetTotalPrice());
        }
        
        public void AddToBasket(Product product)
        {
            if (!_Basket.ContainsKey(product.Id)){
                var b = new BasketItem();
                b.AddProduct(product);
                _Basket.Add(product.Id, b);
            }
            else
            {
                var basketItem = _Basket.FirstOrDefault(b => b.Key == product.Id);
                basketItem.Value.AddItem();
            }
            SaveToSession();
        }

        public void RemoveFromBasket(int id)
        {
            _Basket.Remove(id);
            SaveToSession();
        }

        public int GetBasketCount()
        {
            return _Basket.Sum(b => b.Value.GetQuantity());
        }

        public IDictionary<int,BasketItem> GetBasket()
        {
            //TODO implement potential orderby
            return _Basket;
        }

        public BasketItem GetItemById(int id)
        {
            return _Basket.FirstOrDefault(b => b.Key == id).Value;
        }

        public void SaveToSession()
        {
            var s = JsonConvert.SerializeObject(this);
            _context.HttpContext.Session.SetObjectAsJson(UserId,this);
        }

        public void SetBasket(Dictionary<int,BasketItem> basket)
        {
            _Basket = basket ?? new Dictionary<int, BasketItem>();
            BasketCount = basket.Sum(b => b.Value.GetQuantity());
            SaveToSession();
        }
    }
}

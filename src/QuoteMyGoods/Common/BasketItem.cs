using Newtonsoft.Json;
using QuoteMyGoods.Models;

namespace QuoteMyGoods.Common
{
    [JsonObject]
    public class BasketItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public BasketItem(){}

        public void AddProduct(Product product)
        {
            Product = product;
            Quantity = 1;
            TotalPrice = product.Price;
        }

        public void AddItem()
        {
            TotalPrice += Product.Price;
            Quantity++;
        }
        
        public int RemoveItem()
        {
            TotalPrice -= Product.Price;
            Quantity--;
            return Quantity;
        }

        public decimal GetTotalPrice()
        {
            return TotalPrice;
        }

        public string GetProductName()
        {
            return Product.Name;
        }

        public Product GetProduct()
        {
            return Product;
        }

        public int GetQuantity()
        {
            return Quantity;
        }
    }
}

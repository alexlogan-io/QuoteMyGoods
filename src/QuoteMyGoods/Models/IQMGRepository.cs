using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Models
{
    public interface IQMGRepository
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetProductsByCategory(string categoryName);
        void AddProduct(Product newProduct);
        bool SaveAll();
        Task<Product> GetProductById(int? id);
        Task<SelectList> GetCategories();
        void DeleteProduct(int id);
        void UpdateProduct(Product product);
        Task<int> GetProductCount();
        int CurrentProductCount();
        IEnumerable<string> GetUsers();
    }
}

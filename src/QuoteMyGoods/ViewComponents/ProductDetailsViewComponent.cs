using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class ProductDetailsViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Product product)
        {
            return View(product);
        }
    }
}

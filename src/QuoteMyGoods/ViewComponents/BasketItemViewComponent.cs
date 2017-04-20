using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Common;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class BasketItemViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(BasketItem basketItem)
        {
            return View(basketItem);
        }
    }
}

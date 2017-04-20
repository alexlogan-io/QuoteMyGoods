using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Models;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{

    public class ProductDeleteViewComponent:ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(Product product)
        {
            return View(product);
        }
    }
}

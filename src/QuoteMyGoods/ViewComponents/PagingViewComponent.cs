using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewComponents
{
    public class PagingViewComponent:ViewComponent
    {
        private readonly IQMGRepository _repository;

        public PagingViewComponent(IQMGRepository repository)
        {
            _repository = repository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var itemsPerPage = HttpContext.Request.Query["itemsPerPage"];

            if (!string.IsNullOrWhiteSpace(itemsPerPage))
            {
                ViewBag.itemsPerPage = int.Parse(itemsPerPage);
            }
            else
            {
                ViewBag.itemsPerPage = HttpContext.Session.GetInt32("itemsPerPage") ?? 6;
            }

            var pageNumber = HttpContext.Request.Query["pageNumber"];

            if (!string.IsNullOrWhiteSpace(pageNumber))
            {
                ViewBag.pageNumber = int.Parse(pageNumber);
            }
            else
            {
                ViewBag.pageNumber = HttpContext.Session.GetInt32("pageNumber") ?? 1;
            }

            ViewBag.productCount = _repository.CurrentProductCount();
            return View();
        }
    }
}

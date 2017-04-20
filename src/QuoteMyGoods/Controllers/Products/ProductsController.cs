using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace QuoteMyGoods.Controllers.Products
{
    [Authorize(Roles = "Pleb,Administrator")]
    public class ProductsController:Controller
    {
        private IQMGRepository _repository;
        private IBasketService _basketService;
        private ILoggingService _logger;
        private IRedisService _redisService;
        private UserManager<QMGUser> _userManager;

        public ProductsController(IQMGRepository repository, IBasketService basketService,ILoggingService logger,IRedisService redisService, UserManager<QMGUser> userManager)
        {
            _repository = repository;
            _basketService = basketService;
            _logger = logger;
            _redisService = redisService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Products(string orderbyList, string categoryList, int? itemsPerPage, int? pageNumber)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "GetProducts");
            IEnumerable<Product> products = await _repository.GetProductsByCategory(categoryList);

            if (string.IsNullOrWhiteSpace(itemsPerPage.ToString()))
            {
                itemsPerPage = HttpContext.Session.GetInt32("itemsPerPage") ?? 6;
            }

            if (string.IsNullOrWhiteSpace(pageNumber.ToString()))
            {
                pageNumber = HttpContext.Session.GetInt32("pageNumber") ?? 1;
                if (pageNumber > (products.Count() / itemsPerPage))
                {
                    pageNumber = 1;
                }
            }

            AddInfoToSession((int)itemsPerPage, (int)pageNumber);

            ViewData["categoryList"] = await _repository.GetCategories();

            ViewData["orderbyList"] = new SelectList(new List<string>() { "Name", "Price low-to-high", "Price high-to-low", "Category" });

            if (!string.IsNullOrEmpty(orderbyList))
            {
                products = _OrderBy(products, orderbyList);
            }

            var pagedProducts = products.Skip(((int)pageNumber - 1) * (int)itemsPerPage).Take((int)itemsPerPage);

            //_redisService.SetObject("Products", pagedProducts);
            //var t = _redisService.GetObject<IEnumerable<Product>>("Products");

            return View(pagedProducts);
        }

        private void AddInfoToSession(int itemsPerPage, int pageNumber)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "AddInfoToSession");
            HttpContext.Session.SetInt32("itemsPerPage", itemsPerPage);
            HttpContext.Session.SetInt32("pageNumber", pageNumber);
        }

        private IEnumerable<Product> _OrderBy(IEnumerable<Product> products,string _string)
        {
            switch (_string)
            {
                case "Name":
                    products = products.OrderBy(p => p.Name);
                    break;
                case "Price low-to-high":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "Price high-to-low":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                case "Category":
                    products = products.OrderBy(p => p.Category);
                    break;
                default:
                    break;
            }
            return products;
        }

        public async Task<IActionResult> Details(int? id)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "GetProductDetails");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _repository.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "DeleteProducts");
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "DeleteProductsConfirmed");
            _repository.DeleteProduct(id);
            _repository.SaveAll();
            return RedirectToAction("Products");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "CreateProduct");
            if (ModelState.IsValid)
            {
                _repository.AddProduct(product);
                _repository.SaveAll();
                return RedirectToAction("Products");
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "GetEditProduct");
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _repository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            _logger.Log(_userManager.GetUserId(HttpContext.User), "EditProduct");
            if (ModelState.IsValid)
            {
                _repository.UpdateProduct(product);
                _repository.SaveAll();
                return RedirectToAction("Products");
            }
            return View(product);
        }
    }
}

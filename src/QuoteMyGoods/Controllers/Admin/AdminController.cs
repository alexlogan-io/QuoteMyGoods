using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuoteMyGoods.Models;
using QuoteMyGoods.Services;
using System;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace QuoteMyGoods.Controllers.Admin
{
    public class LoggingDocument
    {
        public string id { get; set; }
        public string UserId { get; set; }
        public string Process { get; set; }
        public DateTime Time { get; set; }

        public LoggingDocument(string userId, string process)
        {
            UserId = userId;
            Process = process;
            Time = DateTime.Now.ToUniversalTime();
        }
    }

    public class AdminController : Controller
    {
        //private MyDocumentDB _db;
        private IQMGRepository _repository;
        private ITableService _tableService;

        public AdminController(IQMGRepository repo, ITableService tableService)
        {
           // _db = new MyDocumentDB(/*"alexpscdb",Startup.Configuration["DocumentDB:ConnectionUri"],Startup.Configuration["DocumentDB:PrimaryKey"]*/);
            _repository = repo;
            _tableService = tableService;
        } 
        // GET: /<controller>/
        public IActionResult Index(string userId)
        {
            /*
            IEnumerable<LoggingDocument> logs =  _db.GetAllDocs("logging") ?? Enumerable.Empty<LoggingDocument>();

            ViewData["userId"] = new SelectList(logs.Select(l => l.UserId).Distinct());

            if (!string.IsNullOrWhiteSpace(userId))
            {
                logs = _db.GetDocsByUserId("logging", userId);
            }
            */
            var logs = Enumerable.Empty<LoggingDocument>();

            return View(logs);
        }

        public IActionResult Details(string id)
        {
            //var log = _db.GetDocById("logging",id);
            var log = default(LoggingDocument);
            return View(log);
        }

        public IActionResult Tables(string partition, string entityType)
        {
            /*
            switch (entityType){
                case "user":
                    IEnumerable<UserEntity> userTables = _tableService.GetTables(partition, new UserEntity().entityResolver) ?? Enumerable.Empty<UserEntity>();
                    return View(userTables);
                default:
                    IEnumerable<ProductEntity> productTables = _tableService.GetTables(partition, new ProductEntity().entityResolver) ?? Enumerable.Empty<ProductEntity>();
                    return View(productTables);
            }
            */
            return View();
        }
    }
}

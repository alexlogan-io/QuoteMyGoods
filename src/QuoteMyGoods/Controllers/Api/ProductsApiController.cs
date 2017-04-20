using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteMyGoods.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace QuoteMyGoods.Controllers.Api
{
    //[Authorize]
    public class ProductsApiController:Controller
    {
        private IQMGRepository _repository;

        public ProductsApiController (IQMGRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet("api/products")]
        public JsonResult Get()
        {
            var products = _repository.GetAllProducts();
            var res = Mapper.Map<IEnumerable<Product>>(products);

            return Json(res);
        }

        [HttpPost("api/products")]
        public JsonResult Post([FromBody]Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newProduct = Mapper.Map<Product>(product);

                    _repository.AddProduct(newProduct);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<Product>(newProduct));
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = ex.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState});
        }

        [HttpDelete("api/product")]
        public JsonResult Delete(int id)
        {
            _repository.DeleteProduct(id);
            if (_repository.SaveAll())
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                return Json(new { Message = "Deleted" });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { Message = "Delete Failed" });
            }
        }

        [HttpGet("api/product")]
        public JsonResult GetById(int id)
        {
            var product = _repository.GetProductById(id);
            return Json(product);
        }

        [HttpPut("api/product")]
        public JsonResult Update([FromBody]Product product)
        {
            if (ModelState.IsValid)
            {
                var productToUpdate = Mapper.Map<Product>(product);
                _repository.UpdateProduct(product);
                if (_repository.SaveAll())
                {
                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json(Mapper.Map<Product>(productToUpdate));
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed to update" });
        }
    }
}

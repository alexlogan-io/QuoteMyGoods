using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.Models
{
    [JsonObject]
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Category { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImgUrl { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}

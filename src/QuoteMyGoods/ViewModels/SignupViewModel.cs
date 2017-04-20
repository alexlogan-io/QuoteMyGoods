using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuoteMyGoods.ViewModels
{
    public class SignupViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password1 { get; set; }
        [Required]
        [Compare("Password1", ErrorMessage ="Passwords don't match.")]
        public string Password2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}

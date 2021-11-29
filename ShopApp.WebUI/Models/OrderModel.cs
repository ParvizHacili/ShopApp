using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class OrderModel
    {
        [Display(Name ="Ad")]
        public string FirstName { get; set; }

        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        [Display(Name = "Ünvan")]
        public string Address { get; set; }

        [Display(Name = "Şəhər")]
        public string City { get; set; }

        [Display(Name = "Əlaqə Nömrəsi")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        public CartModel CartModel { get; set; }
    }
}

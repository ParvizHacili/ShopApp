using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Ad yeri bos ola bilmez")]
        [StringLength(60,ErrorMessage ="Ad 60 simvoldan cox ola bilmez")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Qiymet yeri bos ola bilmez")]
        [Range(1,10000)]
        public decimal? Price { get; set; }

        public string Description { get; set; }
        [Required(ErrorMessage = "Sekil yeri bos ola bilmez")]
        public string ImageUrl { get; set; }
        public bool IsApproved { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}

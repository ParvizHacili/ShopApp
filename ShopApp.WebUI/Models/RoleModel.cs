using ShopApp.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class RoleModel
    {
        [Required(ErrorMessage ="Rol Adı"+UiMessages.RequiredMessage)]
        public string Name { get; set; }
    }
}

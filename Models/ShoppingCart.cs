using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Count = 1;
        }
        public int ID { get; set; }
        public string ApplicationUserID { get; set; }
        [NotMapped]
        [ForeignKey("ApplicationUserID")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        public int MenuItemID { get; set; }
        [NotMapped]
        [ForeignKey("MenuItemID")]
        public virtual MenuItems MenuItems { get; set; }


        [Range(1, 100, ErrorMessage = "Please select a count between one & one hundred!")]
        public int Count { get; set; }
    }
}

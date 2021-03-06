﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual OrderHeader OrderHeader { get; set; }
        public int MenuItemId { get; set; }
        [ForeignKey("MenuItemsId")]
        public virtual MenuItems MenuItems { get; set; }
        public int Count { get; set; }
        public string Name { get; set; }
        public string Descripiton { get; set; }
        [Required]
        public double Price { get; set; }
    }
}


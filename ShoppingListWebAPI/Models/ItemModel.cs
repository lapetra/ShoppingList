using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingListWebAPI.Models
{
    public class ItemModel
    {
        public ItemModel()
        { }

        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
using System;
using System.Collections.Generic;
using ShoppingListCore;
using System.Linq;
using System.Web;

namespace ShoppingListWebAPI.Models
{
    public class ItemListModel
    {
        public int Count;
        public IEnumerable<Item> Data;
    }
}
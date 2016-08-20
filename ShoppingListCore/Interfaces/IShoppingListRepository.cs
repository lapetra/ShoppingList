using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingListCore.Interfaces
{
    public interface IShoppingListRepository
    {
        Item AddItem(Item item);
        Item UpdateItem(Item item);
        Item DeleteItem(string Guid);
        Item GetItem(string name);
        IEnumerable<Item> GetAllItems();

    }
}

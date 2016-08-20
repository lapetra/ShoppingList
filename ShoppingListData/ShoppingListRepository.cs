using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingListCore;
using ShoppingListCore.Interfaces;
using System.Collections.Concurrent;

namespace ShoppingListData
{
    public class ShoppingListRepository:IShoppingListRepository
    {
        private static ConcurrentDictionary<string, Item> _shoppingList =
            new ConcurrentDictionary<string, Item>();
        public Item AddItem(Item item)
        {
            _shoppingList[item.Name.ToLower()] = item;
            return item;
        }

        //Needed to comply with the designed interface
        //If we change the repository to for example a DB, the Update method should be completly different
        //Despite the fact is literally the same that the "AddItem" one with the In memory ConcurrentDictionary.
        public Item UpdateItem(Item item)
        {
            return this.AddItem(item);
        }
        public Item DeleteItem(string name)
        {
            Item item;
            _shoppingList.TryRemove(name.ToLower(), out item);
            return item;
        }
        public Item GetItem(string name)
        {
            try
            {
                return _shoppingList[name.ToLower()];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }

        }

        public IEnumerable<Item> GetAllItems()
        {
            return _shoppingList.Values;
        }
    }
}

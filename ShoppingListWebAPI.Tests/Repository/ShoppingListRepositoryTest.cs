using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingListCore;
using ShoppingListData;

namespace ShoppingListWebAPI.Tests.Repository
{
    [TestClass]
    public class ShoppingListRepositoryTest
    {
        private ShoppingListRepository shoppingListRepository = new ShoppingListRepository();

        [TestMethod]
        public void AddItem()
        {
            // Arrange
            Item item = new Item();
            item.Name = "Pepsi";
            item.Quantity = 10;

            // Act
            item = shoppingListRepository.AddItem(item);

            // Assert The Item Returned By the method
            Assert.IsNotNull(item);
            Assert.AreEqual(10, item.Quantity);
            Assert.AreEqual("Pepsi", item.Name);

            Item savedItem = shoppingListRepository.GetItem("Pepsi");
            Assert.IsNotNull(savedItem);
            Assert.AreEqual(10, savedItem.Quantity);
            Assert.AreEqual("Pepsi", savedItem.Name);
        }

        [TestMethod]
        public void UpdateItem()
        {
            // Arrange
            Item item = new Item();
            item.Name = "Pepsi";
            item.Quantity = 10;

            // Act
            item = shoppingListRepository.AddItem(item);
            item.Quantity = 5;
            Item UpdatedItem = shoppingListRepository.UpdateItem(item);
            // Assert The Item Returned By the method
            Assert.IsNotNull(UpdatedItem);
            Assert.AreEqual(5, UpdatedItem.Quantity);
            Assert.AreEqual("Pepsi", UpdatedItem.Name);


            Item savedItem = shoppingListRepository.GetItem("Pepsi");
            Assert.IsNotNull(savedItem);
            Assert.AreEqual(5, savedItem.Quantity);
            Assert.AreEqual("Pepsi", savedItem.Name);
        }

        [TestMethod]
        public void GetItem()
        {
            // Arrange
            Item item = new Item();
            item.Name = "Chocolate";
            item.Quantity = 3;

            // Act
            item = shoppingListRepository.AddItem(item);
            Item savedItem = shoppingListRepository.GetItem("Chocolate");

            Assert.IsNotNull(savedItem);
            Assert.AreEqual(3, savedItem.Quantity);
            Assert.AreEqual("Chocolate", savedItem.Name);

        }

        [TestMethod]
        public void GetItemIgnoringCase()
        {
            // Arrange
            Item item = new Item();
            item.Name = "IcE CreaM";
            item.Quantity = 1;

            // Act
            item = shoppingListRepository.AddItem(item);
            Item savedItem = shoppingListRepository.GetItem("ice cream");

            Assert.IsNotNull(savedItem);
            Assert.AreEqual(1, savedItem.Quantity);
            Assert.AreEqual("IcE CreaM", savedItem.Name);

        }

        [TestMethod]
        public void TryToGetItemThatDoesNotExist()
        {
            Item savedItem = shoppingListRepository.GetItem("xxxxxx");
            Assert.IsNull(savedItem);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange
            Item item = new Item();
            item.Name = "Milk bottle";
            item.Quantity = 2;

            // Act
            Item savedItem = shoppingListRepository.AddItem(item);
            Item deletedItem = shoppingListRepository.DeleteItem("Milk bottle");

            Assert.IsNull(shoppingListRepository.GetItem("Milk bottle"));
        }

        [TestMethod]
        public void DeleteItemCaseInsensitive()
        {
            // Arrange
            Item item = new Item();
            item.Name = "MilK BOttLe";
            item.Quantity = 2;

            // Act
            Item savedItem = shoppingListRepository.AddItem(item);
            Item deletedItem = shoppingListRepository.DeleteItem("milk bottle");
            Assert.IsNull(shoppingListRepository.GetItem("Milk bottle"));
        }

        [TestMethod]
        public void GetAll()
        {
            Item item2 = new Item();
            item2.Name = "Apple";
            item2.Quantity = 5;
            item2 = shoppingListRepository.AddItem(item2);

            IEnumerable<Item> shoppingList = shoppingListRepository.GetAllItems();

            Assert.IsNotNull(shoppingList);
            Assert.IsTrue(shoppingList.Count<Item>()>0);

        }
    }
}

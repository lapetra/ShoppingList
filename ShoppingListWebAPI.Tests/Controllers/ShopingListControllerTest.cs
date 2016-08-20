using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingListWebAPI;
using ShoppingListWebAPI.Controllers;
using ShoppingListWebAPI.Models;
using ShoppingListCore;
using ShoppingListCore.Interfaces;
using ShoppingListData;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Generic;

namespace ShoppingListWebAPI.Tests.Controllers
{
    [TestClass]
    public class ShoppingListControllerTest
    {

        ShoppingListController controller;
        ShoppingListRepository repository;

        public ShoppingListControllerTest()
        {
            repository = new ShoppingListRepository();
            controller = new ShoppingListController(repository);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

        }

        [TestMethod]
        public void AddItem()
        {
            ItemModel postedItem = new ItemModel();
            postedItem.Name = "Pepsi";
            postedItem.Quantity = 3;
            // Act
            HttpResponseMessage response = controller.AddItem(postedItem);
            
            // Assert Ok Status code
            Assert.AreEqual(response.StatusCode,System.Net.HttpStatusCode.OK);
            
            // Assert Item
            Item item;
            Assert.IsTrue(response.TryGetContentValue<Item>(out item));
            Assert.AreEqual(3, item.Quantity);
            Assert.AreEqual("Pepsi", item.Name);

            // Ensure its saved correctly in the "Data Repository"
            Item savedItem = repository.GetItem("pepsi");
            Assert.AreEqual("Pepsi", savedItem.Name);
            Assert.AreEqual(3, savedItem.Quantity);
        }

        [TestMethod]
        public void AddItemWithEmptyName()
        {
            ItemModel postedItem = new ItemModel();
            postedItem.Name = string.Empty;
            postedItem.Quantity = 3;
            // Act
            HttpResponseMessage response = controller.AddItem(postedItem);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        public void AddNullItem()
        {
            // Act
            HttpResponseMessage response = controller.AddItem(null);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void AddItemWithQuantityLessThan1()
        {
            ItemModel postedItem = new ItemModel();
            postedItem.Name = "Milk";
            postedItem.Quantity = -1;
            // Act
            HttpResponseMessage response = controller.AddItem(postedItem);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void GetItem()
        {
            // Act
            Item repositoryItem = new Item();
            repositoryItem.Name = "Chocolate";
            repositoryItem.Quantity = 6;
            repository.AddItem(repositoryItem);

            HttpResponseMessage response = controller.GetItem("chocolate");
            // Assert Ok Status code
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            Item item;
            Assert.IsTrue(response.TryGetContentValue<Item>(out item));
            Assert.AreEqual(6, item.Quantity);
            Assert.AreEqual("Chocolate", item.Name);
        }

        [TestMethod]
        public void GetItemWithEmptyName()
        {
            // Act
            HttpResponseMessage response = controller.GetItem("");
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        public void GetNonExistantItem()
        {
            // Act
            HttpResponseMessage response = controller.GetItem("xxx");
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.NotFound);
        }


        [TestMethod]
        public void UpdateItem()
        {
            // Act
            Item repositoryItem = new Item();
            repositoryItem.Name = "ice cream";
            repositoryItem.Quantity = 6;
            repository.AddItem(repositoryItem);

            // Act
            ItemModel sentItem = new ItemModel();
            sentItem.Name = "ice cream";
            sentItem.Quantity = 10;

            HttpResponseMessage response = controller.UpdateItem(sentItem);

            // Assert Ok Status code
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            // Assert Item
            Item item;
            Assert.IsTrue(response.TryGetContentValue<Item>(out item));
            Assert.AreEqual(10, item.Quantity);
            Assert.AreEqual("ice cream", item.Name);


            
            // Ensure its saved correctly in the "Data Repository"
            Item savedItem = repository.GetItem("ice cream");
            Assert.AreEqual("ice cream", savedItem.Name);
            Assert.AreEqual(10, savedItem.Quantity);
        }

        [TestMethod]
        public void UpdateItemWithEmptyName()
        {
            ItemModel postedItem = new ItemModel();
            postedItem.Name = string.Empty;
            postedItem.Quantity = 3;
            // Act
            HttpResponseMessage response = controller.UpdateItem(postedItem);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        public void UpdateNullItem()
        {
            // Act
            HttpResponseMessage response = controller.UpdateItem(null);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void UpdateItemWithQuantityLessThan1()
        {
            ItemModel postedItem = new ItemModel();
            postedItem.Name = "Milk";
            postedItem.Quantity = -1;
            // Act
            HttpResponseMessage response = controller.UpdateItem(postedItem);
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Act
            Item repositoryItem = new Item();
            repositoryItem.Name = "Beer";
            repositoryItem.Quantity = 100;
            repository.AddItem(repositoryItem);

            // Act


            HttpResponseMessage response = controller.DeleteItem("beer");

            // Assert Ok Status code
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);

            // Assert Item
            Item deletedItem;
            Assert.IsTrue(response.TryGetContentValue<Item>(out deletedItem));
            Assert.AreEqual(100, deletedItem.Quantity);
            Assert.AreEqual("Beer", deletedItem.Name);

            // Ensure it was removed correctly from  the "Data Repository"
            Assert.IsNull(repository.GetItem("Beer"));
        }

        [TestMethod]
        public void DeleteItemWithEmptyName()
        {
            // Act
            HttpResponseMessage response = controller.DeleteItem("");
            // Assert Bad request
            Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void GetAll()
        {
            Item item1 = new Item();
            item1.Name = "Apple";
            item1.Quantity = 5;
            item1 = repository.AddItem(item1);

            Item item2 = new Item();
            item2.Name = "Apple";
            item2.Quantity = 5;
            item2 = repository.AddItem(item2);

            ItemListModel shoppingList = controller.GetAllItems();

            Assert.IsNotNull(shoppingList);
            Assert.IsTrue(shoppingList.Count > 0);

        }

    }
}

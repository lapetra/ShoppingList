using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ShoppingListCore;
using ShoppingListCore.Interfaces;
using ShoppingListData;
using ShoppingListWebAPI.Models;

namespace ShoppingListWebAPI.Controllers
{
    public class ShoppingListController : ApiController
    {
        public IShoppingListRepository ShoppingListRepository { get; set; }
        public ShoppingListController(IShoppingListRepository shoppingListRepository)
        {
            ShoppingListRepository = shoppingListRepository;
        }


        [HttpPost]
        public HttpResponseMessage AddItem([FromBody]ItemModel postedItem)
        {
            if (postedItem == null || string.IsNullOrEmpty(postedItem.Name))
                return CreateNameErrorResponse();
            if (postedItem.Quantity < 1)
                return CreateQuanityErrorResponse();

            Item newItem = new Item();
            newItem.Quantity = postedItem.Quantity;
            newItem.Name = postedItem.Name;

            Item savedItem = ShoppingListRepository.AddItem(newItem);

            if (savedItem == null)
                return CreateRepositoryErrorResponse(postedItem.Name);

            return Request.CreateResponse(HttpStatusCode.OK, savedItem);
        }

        [HttpGet]
        public HttpResponseMessage GetItem(string name)
        {

            if (string.IsNullOrEmpty(name))
                return CreateNameErrorResponse();

            Item item = ShoppingListRepository.GetItem(name);

            if (item == null)
                return CreateItemNotFoundErrorResponse(name);

            return Request.CreateResponse(HttpStatusCode.OK, item);
        }


        [HttpGet]
        public ItemListModel GetAllItems()
        {
            ItemListModel itemList = new ItemListModel();
            itemList.Data = ShoppingListRepository.GetAllItems();
            itemList.Count = itemList.Data.Count();
            return itemList;
        }

        [HttpDelete]
        public HttpResponseMessage DeleteItem(string name)
        {
            if (string.IsNullOrEmpty(name))
                return CreateNameErrorResponse();

            Item deletedItem = ShoppingListRepository.DeleteItem(name);

            if (deletedItem == null)
                return CreateItemNotFoundErrorResponse(name);
            else
                return Request.CreateResponse(HttpStatusCode.OK, deletedItem);
        }

        [HttpPut]
        public HttpResponseMessage UpdateItem(ItemModel updatedItem)
        {
            if (string.IsNullOrEmpty(updatedItem.Name))
                return CreateNameErrorResponse();

            if (updatedItem.Quantity < 1)
                return CreateQuanityErrorResponse();


            Item itemToUpdate = ShoppingListRepository.GetItem(updatedItem.Name);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = updatedItem.Quantity;

                Item savedItem = ShoppingListRepository.UpdateItem(itemToUpdate);
                if (savedItem == null)
                    return CreateRepositoryErrorResponse(itemToUpdate.Name);
                else
                    return Request.CreateResponse(HttpStatusCode.OK, savedItem);
            }
            else
                 return CreateItemNotFoundErrorResponse(itemToUpdate.Name); 
        }

        [NonAction]
        private HttpResponseMessage CreateNameErrorResponse()
        {
            var message = "Item name can't be null or empty";
            HttpError err = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.BadRequest, err);
        }

        [NonAction]
        private HttpResponseMessage CreateQuanityErrorResponse()
        {
            var message = "Quantity needs to be larger than 0";
            HttpError err = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.BadRequest, err);

        }

        [NonAction]
        private HttpResponseMessage CreateRepositoryErrorResponse(string name)
        {
            var message = string.Format("Item with name = {0} could not be saved", name);
            HttpError err = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
        }

        [NonAction]
        private HttpResponseMessage CreateItemNotFoundErrorResponse(string name)
        {
            var message = string.Format("Item with name = {0} could not be found", name);
            HttpError err = new HttpError(message);
            return Request.CreateResponse(HttpStatusCode.NotFound, err);
        }
    }
}

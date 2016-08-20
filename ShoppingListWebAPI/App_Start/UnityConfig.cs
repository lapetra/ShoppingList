using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;
using ShoppingListCore;
using ShoppingListCore.Interfaces;
using ShoppingListData;

namespace ShoppingListWebAPI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IShoppingListRepository, ShoppingListRepository>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
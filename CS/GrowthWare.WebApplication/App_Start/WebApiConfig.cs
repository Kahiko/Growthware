using GrowthWare.WebApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace GrowthWare.WebApplication
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }

        public static void RegisterRoutes(RouteCollection routes) 
        {
            Route sessionRoute = null;
            string sessionUrlPattern = string.Empty;

            sessionUrlPattern = "api/{controller}/{action}/{id}";
            sessionRoute = new Route(sessionUrlPattern, new SessionStateRouteHandler());
            sessionRoute.Defaults = new RouteValueDictionary(new { id = RouteParameter.Optional });
            routes.Add("DefaultApi", sessionRoute);
        }
    }
}

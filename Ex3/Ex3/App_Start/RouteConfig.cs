using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace Ex3
{
    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "default",
                url: "{controller}/{action}",
                defaults: new { controller = "Main", action = "display" }
            );

            routes.MapRoute(
                name: "displayLine",
                url: "display/{ip}/{port}/{seconds}",
                defaults: new { controller = "Main", action = "displayLine"}
            );

            routes.MapRoute(
               name: "display",
               url: "display/{ip}/{port}",
               defaults: new { controller = "Main", action = "display" }
           );
        }
    }
}

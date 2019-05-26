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

            routes.MapRoute("save", "save/{ip}/{port}/{refresh}/{seconds}/{name}",
            defaults: new { controller = "Main", action = "save" });
            

            routes.MapRoute("display", "display/{ip}/{port}",
            defaults: new { controller = "Main", action = "display" });
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "main", id = UrlParameter.Optional }
            );
        }
    }
}

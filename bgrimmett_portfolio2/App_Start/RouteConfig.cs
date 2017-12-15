using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace bgrimmett_portfolio2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // blog post slug route - Edit
            routes.MapRoute(
                name: "EditSlug",
                url: "Posts/Edit/{slug}",
                defaults: new
                {
                    controller = "Posts",
                    action = "Edit",
                    slug = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "NewSlug",
                url: "Blog/{slug}",
                defaults: new
                {
                    controller = "Posts",
                    action = "Details",
                    slug = UrlParameter.Optional
                });



            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

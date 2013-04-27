using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CIS726_Assignment2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "SearchCourses",
                url: "Courses/SearchCourses/{term}",
                defaults: new { controller = "Courses", action = "SearchCourses", term = "" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Courses", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
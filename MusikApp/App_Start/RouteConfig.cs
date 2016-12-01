using System.Web.Mvc;
using System.Web.Routing;

namespace MusikApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("tag", "tag/{tag}", new {controller = "music", action = "GetTopTags"});
            routes.MapRoute("artist", "artist/{query}", new { controller = "music", action = "SearchArtist" });
            routes.MapRoute("Searchartist", "SearchArtist/{query}", new { controller = "music", action = "SearchArtist" });
            routes.MapRoute("search", "search/{query}", new { controller = "music", action = "Search" });
            routes.MapRoute("typeahead", "typeahead/{query}", new { controller = "music", action = "Typeahead" });
            routes.MapRoute("youtube", "music/GetYoutube/{track}/{artist}", new { controller = "music", action = "GetYoutube" });
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
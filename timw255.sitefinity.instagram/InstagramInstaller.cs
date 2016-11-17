using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using Telerik.Sitefinity.Abstractions;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Data;
using timw255.Sitefinity.Instagram.Mvc.Configuration;

namespace timw255.Sitefinity.Instagram
{
    public static class InstagramInstaller
    {
        public static void PreApplicationStart()
        {
            Bootstrapper.Initialized += InstagramInstaller.OnBootstrapperInitialized;
        }

        private static void OnBootstrapperInitialized(object sender, ExecutedEventArgs e)
        {
            if (e.CommandName == "Bootstrapped")
            {
                RegisterRoutes(RouteTable.Routes);

                Config.RegisterSection<InstagramConfig>();
            }
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "Instagram",
                routeTemplate: "Instagram/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );
        }
    }
}

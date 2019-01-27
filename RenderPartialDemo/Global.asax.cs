using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using RenderPartialDemo.Migrations;
using RenderPartialDemo.Models;

namespace RenderPartialDemo
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new MigrateDatabaseToLatestVersion<GreetingsDbContext, Configuration>());
        }
    }
}

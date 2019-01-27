using RenderPartialDemo.Hubs;
using RenderPartialDemo.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RenderPartialDemo.Helpers
{
    public class Render
    {
        public static void RenderGreeting(int GreetingID)
        {
            using (var _db = new GreetingsDbContext())
            {
                var greeting = _db.Greetings.SingleOrDefault(x => x.Id == GreetingID);

                if (string.IsNullOrEmpty(greeting.RawHtml))
                {
                    greeting.RawHtml = RenderViewToString("~/Views/Hello/_Hello.cshtml", greeting, true);
                    greeting.DateProcessed = DateTime.Now;

                    _db.SaveChanges();

                    var hub = new GreetingsHub();
                    hub.Send(greeting.Id);
                }
            }
        }

        private static ControllerContext CreateEmptyControllerContext(RouteData routeData = null)
        {
            var controller = new EmptyController();
            HttpContextBase wrapper = new HttpContextWrapper(CreateHttpContext());

            routeData = routeData ?? new RouteData();

            if (!routeData.Values.ContainsKey("controller"))
            {
                var value = controller.GetType().Name.ToLower().Replace("controller", "");
                routeData.Values.Add("controller", value);
            }

            return new ControllerContext(wrapper, routeData, controller);
        }

        public static string RenderViewToString(string viewPath, object model = null, bool partial = false)
        {
            var context = CreateEmptyControllerContext();
            ViewEngineResult viewEngineResult = null;

            viewEngineResult = partial
                ? ViewEngines.Engines.FindPartialView(context, viewPath)
                : ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
            {
                throw new FileNotFoundException($"Could not find view at {viewPath}");
            }

            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;

        }

        private static HttpContext CreateHttpContext()
        {
            return new HttpContext(
                new HttpRequest("", "http://example.com", ""),
                new HttpResponse(new StringWriter())
            );
        }
    }

    public class EmptyController : Controller
    { }
}
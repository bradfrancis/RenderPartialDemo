using System;
using System.Linq;
using System.Web.Mvc;
using RenderPartialDemo.Models;

namespace RenderPartialDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly GreetingsDbContext _db = new GreetingsDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var greetings = _db.Greetings.OrderBy(x => x.Id).ToList();
            return View(greetings);
        }

        [HttpPost]
        public PartialViewResult Create(Greeting model)
        {
            if (ModelState.IsValid)
            {
                _db.Greetings.Add(model);
                _db.SaveChanges();

                Hangfire.BackgroundJob.Schedule(() => Helpers.Render.RenderGreeting(model.Id), TimeSpan.FromSeconds(5));
            }

            return PartialView("_GreetingsList", _db.Greetings.OrderBy(x => x.Id).ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
using RenderPartialDemo.Models;
using Microsoft.AspNet.SignalR;
using System.Linq;

namespace RenderPartialDemo.Hubs
{
    public class GreetingsHub : Hub
    {
        private readonly GreetingsDbContext _db = new GreetingsDbContext();

        public void Send(int ID)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<GreetingsHub>();
            if (hubContext != null)
            {
                Greeting model;
                if ((model = _db.Greetings.FirstOrDefault(x => x.Id == ID)) != null)
                {
                    var html = Helpers.Render.RenderViewToString("~/Views/Home/_Greeting.cshtml", model, true);
                    hubContext.Clients.All.sendGreeting(new
                    {
                        ElementId = $"greeting-{model.Id}",
                        Html = html
                    });
                }
            }
        }
    }
}
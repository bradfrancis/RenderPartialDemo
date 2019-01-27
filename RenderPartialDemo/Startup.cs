using Hangfire;
using Hangfire.SqlServer;
using Owin;
using System;

namespace RenderPartialDemo
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration
                .UseSqlServerStorage(
                    "GreetingsDb",
                    new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });

            app.MapSignalR();
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
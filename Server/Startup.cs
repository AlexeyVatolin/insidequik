using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace Server
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration
            {
#if DEBUG
                EnableDetailedErrors = true
#else
                EnableDetailedErrors = false
#endif
            };
            app.MapSignalR("/signalr", hubConfiguration);
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}

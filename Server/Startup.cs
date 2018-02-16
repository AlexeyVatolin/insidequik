using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using ServerCore.Base.DatabaseContext;

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
            Context.Init();
            app.MapSignalR("/signalr", hubConfiguration);
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }
}

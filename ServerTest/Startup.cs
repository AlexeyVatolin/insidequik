using Microsoft.AspNet.SignalR;
using Owin;

namespace ServerTest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            //GlobalHost.HubPipeline.RequireAuthentication();
        }
    }
}
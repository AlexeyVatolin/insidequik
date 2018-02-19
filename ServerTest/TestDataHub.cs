using Microsoft.AspNet.SignalR;

namespace ServerTest
{
    public class TestDataHub : Hub
    {
        private string CurrentUser => SessionStorage.GetUser(Context.Headers["sessionId"]);

        public string SayHello()
        {
            var user = CurrentUser;
            return "Hello!";
        }
    }
}

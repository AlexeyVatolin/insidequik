using System.Net;
using Microsoft.AspNet.SignalR.Hubs;

namespace ServerCore.Hubs.Extensions
{
    public static class HubExtensions
    {
        public static string LocalhostIPv4 => IPAddress.Loopback.ToString();

        public static string LocalhostIPv6 => IPAddress.IPv6Loopback.ToString();

        public static string GetConnectionIp(this IHub hub)
        {
            if (!hub.Context.Request.Environment.TryGetValue("server.RemoteIpAddress", out var address))
            {
                return null;
            }
            var ip = (string)address;
            return ip.Equals(LocalhostIPv6) ? LocalhostIPv4 : ip;
        }
    }
}

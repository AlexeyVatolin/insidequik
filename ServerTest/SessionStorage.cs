using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    public static class SessionStorage
    {
        private static ConcurrentDictionary<string, string> sessions = new ConcurrentDictionary<string, string>();

        public static void Add(string sessionId, string user)
        {
            sessions.AddOrUpdate(sessionId, s => user, (s, s1) => user);
        }

        public static string GetUser(string sessionId)
        {
            sessions.TryGetValue(sessionId, out var user);
            return user;
        }
    }
}

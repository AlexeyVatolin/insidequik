using System;
using System.Collections.Concurrent;
using ServerCore.Base.DatabaseContext.Entities;

namespace ServerCore.Hubs.Storages
{
    public static class SessionStorage
    {
        private static ConcurrentDictionary<string, User> sessions = new ConcurrentDictionary<string, User>();

        public static string CreateSession(User user)
        {
            string sessionId = Guid.NewGuid().ToString();
            sessions.TryAdd(sessionId, user);
            return sessionId;
        }

        public static User GetUser(string sessionId)
        {
            sessions.TryGetValue(sessionId, out var user);
            return user;
        }

        public static void FinishSession(string sessionId)
        {
            sessions.TryRemove(sessionId, out var user);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using ServerCore.Hubs.Interfaces;

namespace ServerCore.Hubs.Models
{
    class ConnectedClients
    {
        private readonly List<IClientBase> _clients = new List<IClientBase>();
        private readonly object _syncRoot = new object();

        public void Add(IClientBase client)
        {
            lock (_syncRoot)
            {
                _clients.Add(client);
            }
        }

        public void Remove(IClientBase client)
        {
            lock (_syncRoot)
            {
                _clients.Remove(client);
            }
        }
        public IClientBase GetClientByConnectionId(string id)
        {
            lock (_syncRoot)
            {
                return _clients.FirstOrDefault(c => c.ConnectionId == id);
            }
        }
        public IClientBase GetClientByName(string name)
        {
            lock (_syncRoot)
            {
                return _clients.FirstOrDefault(c => c.Name.EqualsIgnoreCase(name));
            }
        }
        public IEnumerable<IClientBase> GetClientsByName(string name)
        {
            lock (_syncRoot)
            {
                return _clients.Where(c => c.Name.EqualsIgnoreCase(name));
            }
        }
        public List<IClientBase> Disconnect(string connectionId, bool isDisconnected)
        {
            return Disconnect(c => c.ConnectionId == connectionId, isDisconnected);
        }
        public List<IClientBase> Disconnect(Func<IClientBase, bool> predicate, bool isDisconnected)
        {
            lock (_syncRoot)
            {
                var clients = _clients.Where(predicate).ToList();
                if (!isDisconnected) return clients;
                foreach (var client in clients)
                {
                    _clients.Remove(client);
                }
                return clients;
            }
        }
        public List<IClientBase> GetClients(Func<IClientBase, bool> predicate)
        {
            lock (_syncRoot)
            {
                var clients = _clients.Where(predicate).ToList();
                return clients;
            }
        }
        public void RemoveClients(Func<IClientBase, bool> predicate, bool isDisconnected)
        {
            lock (_syncRoot)
            {
                var clients = _clients.Where(predicate).ToList();
                if (!isDisconnected) return;
                foreach (var client in clients)
                {
                    _clients.Remove(client);
                }
            }
        }
        public IClientBase[] OnlineClients
        {
            get
            {
                lock (_syncRoot)
                {
                    return _clients.OrderBy(a => a.LastAccessDate).ToArray();
                }
            }
        }

        ///// <summary>
        ///// Connection Id of current hub object
        ///// </summary>
        //private string ConnectionId => Context.ConnectionId;

        //private IClientBase _client;
        //private IClientBase Client => _client ?? (_client = GetClientByConnectionId(ConnectionId));

        //protected IClientBase GetCurrentClient()
        //{
        //    var client = Client;
        //    if (client == null)
        //    {
        //        throw new NotLoggedException();
        //    }

        //    if (client.UserStatus != UserStatus.Active)
        //    {
        //        lock (SyncRoot)
        //        {
        //            _clients.Remove(client);
        //        }
        //        CurrentContext.Logout(client.ConnectionId, null);
        //        throw new UserIsNotActiveException();
        //    }

        //    client.LastAccessDate = DateTime.UtcNow;
        //    return client;
        //}
    }
}

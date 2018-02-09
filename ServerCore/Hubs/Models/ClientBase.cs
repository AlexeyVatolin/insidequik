using System;
using ServerCore.Base.DatabaseContext.Entities;
using ServerCore.Hubs.Interfaces;

namespace ServerCore.Hubs.Models
{
    /// <summary>
    /// ClientBase class
    /// </summary>
    /// <property name="Name">string Client name</property>
    /// <property name="ConnectionId">string unique id Hub connection</property>
    /// <property name="ApplicationUserId">string unique id of account</property>
    public class ClientBase : IClientBase
    {
        public string Name { get; set; }
        public string ConnectionId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Ip { set; get; }
        /// <summary>
        ///  user status
        /// </summary>
        public UserStatus UserStatus { set; get; }
        public DateTime LastAccessDate { set; get; }
        public object Clone()
        {
            return MemberwiseClone();
        }
        public TClient Clone<TClient>() where TClient : ClientBase, new()
        {
            return (TClient)Clone();
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

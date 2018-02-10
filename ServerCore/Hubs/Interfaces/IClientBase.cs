using System;
using ServerCore.Base.DatabaseContext.Entities;

namespace ServerCore.Hubs.Interfaces
{
    public interface IClientBase : ICloneable
    {
        /// <summary>
        ///  client name
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// unique client connection Guid/Id
        /// </summary>
        string ConnectionId { get; set; }
        /// <summary>
        /// Id from db
        /// </summary>
        string ApplicationUserId { set; get; }
        string Ip { set; get; }
        DateTime LastAccessDate { set; get; }
        /// <summary>
        /// User status
        /// </summary>
        UserStatus UserStatus { set; get; }
    }
}

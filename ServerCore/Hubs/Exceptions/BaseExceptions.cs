using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Common.Extensions;
using Common.Models;
using Microsoft.AspNet.SignalR;

namespace ServerCore.Hubs.Exceptions
{
    public class ExceptionData
    {
        public ExceptionData(ErrorCodes code)
        {
            Code = code;
        }

        public ErrorCodes Code { set; get; }
    }

    [Serializable]
    public class ExceptionBase : HubException
    {
        public ExceptionBase()
        { }

        public ExceptionBase(string message) : base(message)
        {
        }

        public ExceptionBase(string message, ExceptionData errorData) : base(message, errorData)
        {
            HResult = (int) errorData.Code;
        }

        public ExceptionBase(string message, ErrorCodes errorCode) : this(message, new ExceptionData(errorCode))
        { }

        public ExceptionBase(ErrorCodes errorCode) : this(errorCode.GetDescription(), errorCode)
        { }

        /// <summary>
        ///  This protected constructor is used for deserialization.
        /// </summary>
        protected ExceptionBase(SerializationInfo info, StreamingContext context) : base(info.ToString(), context)
        { }

        /// <summary>
        /// GetObjectData performs a custom serialization.
        /// </summary>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }

    public class NotLoggedException : ExceptionBase
    {
        public NotLoggedException() : base(ErrorCodes.NotLogged)
        { }
    }

    public class UserIsNotActiveException : ExceptionBase
    {
        public UserIsNotActiveException() : base(ErrorCodes.UserIsNotActive)
        { }
    }

    public class CantConvertToJsonObjectException : ExceptionBase
    {
        public CantConvertToJsonObjectException() : base(ErrorCodes.CantConvertToJsonObject) { }
    }

    public class WrongUsernameOrPasswordException : ExceptionBase
    {
        public WrongUsernameOrPasswordException() : base(ErrorCodes.WrongUserNameOrPassword) { }
    }
}

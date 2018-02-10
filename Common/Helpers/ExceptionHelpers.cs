using System;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json.Linq;

namespace Inside.Common.Helpers.Extensions
{
    public static class ExceptionHelpers
    {
        public static string GetMessage(this Exception self)
        {
            return self.GetInnerException().Message;
        }
        public static Exception GetInnerException(this Exception self)
        {
            return self.InnerException != null ? GetInnerException(self.InnerException) : self;
        }
        public static T GetException<T>(this Exception self) where T : Exception
        {
            if (self == null)
            {
                return null;
            }

            var exception = self as T;
            return exception ?? GetException<T>(self.InnerException);
        }
        public static int GetServerExceptionCode<T>(this Exception self) where T : HubException
        {
            var exception = self.GetException<T>();
            if (exception == null) return 0;
            var jObj = (JObject)exception.ErrorData;
            return (int)(jObj["HResult"] ?? jObj["Code"]);
        }
    }
}

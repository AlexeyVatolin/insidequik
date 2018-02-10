using System;
using Common.Interfaces;
using Newtonsoft.Json.Linq;
using ServerCore.Hubs.Exceptions;

namespace ServerCore.Helpres
{
    public static class JObjectHelper
    {
        public static JObject GetJObject(object value)
        {
            if (!(value is JObject result))
            {
                throw new CantConvertToJsonObjectException();
            }
            return result;
        }

        public static TResult GetJObject<TResult, TException>(object value)
            where TResult : IValidate
            where TException : Exception, new()
        {
            var jObject = GetJObject(value);
            var result = (TResult)jObject.ToObject(typeof(TResult));
            if (!result.IsValid)
            {
                throw new TException();
            }
            return result;
        }

        public static TResult Cast<TResult>(this object self)
        {
            return GetJObject<TResult>(self);
        }

        public static TResult GetJObject<TResult>(object value)
        {
            var jObject = GetJObject(value);
            return (TResult)jObject.ToObject(typeof(TResult));
        }
    }
}

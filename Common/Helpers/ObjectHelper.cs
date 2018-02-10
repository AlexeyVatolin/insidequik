using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Helpers
{
    public static class ObjectHelper
    {
        public static string ToJson(this object self)
        {
            return JsonConvert.SerializeObject(self);
        }
        /// <summary>
        /// Cast from one object type to another using JSON serialization.
        /// Warning. Not use this method when one or more fields in object has type "object"
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="self">Object that need to cast</param>
        /// <returns>Object of type T</returns>
        public static T ToObject<T>(this object self)
        {
            if (self == null)
                return default(T);

            var jObject = self as JObject;
            if (jObject != null) return (T)jObject.ToObject(typeof(T));
            jObject = JObject.Parse(self.ToString());
            return jObject == null ? (T)self : (T)jObject.ToObject(typeof(T));
        }
        /// <summary>
        /// Cast from one object type to another where T type derived from parameter object type using JSON serialization.
        /// Warning. Not use this method when one or more fields in object has type "object"
        /// </summary>
        /// <typeparam name="T">Any type</typeparam>
        /// <param name="obj">Object that need to cast</param>
        /// <returns>Object of type T</returns>
        public static T CastToDerivedType<T>(this object obj)
        {
            var baseType = obj.GetType();
            var derivedType = typeof(T);
            if (derivedType.IsSubclassOf(baseType))
                return obj.CastRaw<T>();
            throw new InvalidOperationException($"Type {derivedType.Name} is not derived from {baseType.Name}");
        }
        public static T CastRaw<T>(this object obj)
        {
            return JsonConvert
                .DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }
    }
}

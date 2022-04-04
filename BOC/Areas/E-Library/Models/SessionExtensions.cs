using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace BOC.Models
{
    public static class SessionExtensions
    {
        public static T GetListData<T>(this ISession session, string key)
        {
            var data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static void SetListData(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}

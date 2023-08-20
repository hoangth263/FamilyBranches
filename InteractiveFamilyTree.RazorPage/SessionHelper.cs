using System.Text.Json;

namespace InteractiveFamilyTree.RazorPage
{
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }

        public static void SetStringToSession(this ISession session, string key, string value)
        {
            session.SetString(key, value);
            return;
        }

        public static string GetStringFromSession(this ISession session, string key)
        {
            bool check = session.TryGetValue(key, out var sessionVar);
            if (check)
            {
                return System.Text.Encoding.UTF8.GetString(sessionVar);
            }
            return null;
        }

        public static int GetIntFromSession(this ISession session, string key)
        {
            bool check = session.TryGetValue(key, out var sessionVar);
            if (check)
            {
                return int.Parse(System.Text.Encoding.UTF8.GetString(sessionVar));
            }
            return 0;
        }

        public static void RemoveStringFromSession(this ISession session, string key)
        {
            session.Remove(key);
        }
    }
}

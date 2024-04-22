using Newtonsoft.Json;

namespace JITs.Services.Helpers;

public static class Converter
{
    private static Object GetObject(this Dictionary<string, object> dict, Type type)
    {
        var obj = Activator.CreateInstance(type);

        foreach (var kv in dict)
        {
            var prop = type.GetProperty(kv.Key);
            if (prop == null) continue;

            object value = kv.Value;
            if (value is Dictionary<string, object>)
            {
                value = GetObject((Dictionary<string, object>)value, prop.PropertyType); // <= This line
            }

            if (prop.PropertyType.Name == "DateTime")
            {
                Timestamp ts = (Timestamp)value;
                value = ts.ToDateTime();
            }
            if (prop.PropertyType.Name == "Int32") value = Convert.ToInt32(value);
            prop.SetValue(obj, value, null);
        }
        return obj;
    }

    public static T GetObject<T>(this Dictionary<string, object> dict)
    {
        return (T)GetObject(dict, typeof(T));
    }

    public static Dictionary<string, object> UserToDict(User user)
    {
        var userdatadict = JsonConvert.SerializeObject(user);
        var usernamedict = JsonConvert.SerializeObject(user.Name);
        var userclassdict = JsonConvert.SerializeObject(user.Class);

        var userdatades = JsonConvert.DeserializeObject<Dictionary<string, object>>(userdatadict);
        var usernamedes = JsonConvert.DeserializeObject<Dictionary<string, object>>(usernamedict);
        var userclassdes = JsonConvert.DeserializeObject<Dictionary<string, object>>(userclassdict);

        userdatades["Name"] = usernamedes;
        userdatades["Class"] = userclassdes;

        userdatades.Remove("DocId");

        return userdatades;
    }
}

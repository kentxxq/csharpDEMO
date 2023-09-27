using System.Text.Json;

namespace MyConvert;

public static class MyJsonConvert
{
    public static string ObjectToString(this object o)
    {
        var json = JsonSerializer.Serialize(o, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        return json;
    }
}

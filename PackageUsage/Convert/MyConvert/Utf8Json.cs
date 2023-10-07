using System.Text;
using System.Text.Json;

namespace MyConvert;

public static class Utf8Json
{
    public static void TestUtf8Json()
    {
        var options = new JsonWriterOptions
        {
            Indented = true
        };

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, options);

        writer.WriteStartObject();
        writer.WriteString("date", DateTimeOffset.UtcNow);
        writer.WriteNumber("temp", 42);
        // writer.WriteEndObject(); // 不输出结尾的大括号
        writer.Flush();
        var json = Encoding.UTF8.GetString(stream.ToArray());
        Console.WriteLine(json);

        // 读取这个不完整的json
        // 因为最后的42没有截止符,无法读取
        var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(json), false, default);
        while (reader.Read())
        {
            Console.WriteLine(reader.TokenType);
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var property = reader.GetString();
                if (property == "date")
                {
                    reader.Read();
                    Console.WriteLine($"{reader.TokenType}: {reader.GetDateTime()}");
                }
            }
        }
    }
}

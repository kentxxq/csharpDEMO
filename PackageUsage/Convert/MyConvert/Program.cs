// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using DemoData;
using DemoData.Models;
using MyConvert;

#region datetime转timestamp

// Console.WriteLine($"测试{StaticData.DatetimeData}转timestamp");
// Console.WriteLine(StaticData.DatetimeData.ToTimestampSeconds());
// Console.WriteLine(StaticData.DatetimeData.ToTimestampMilliseconds());

#endregion

#region Timestamp转datetime

// Console.WriteLine($"测试{StaticData.TimestampSecondsData}转datetime");
// Console.WriteLine(StaticData.TimestampSecondsData.SecondsToDateTime());
//
// Console.WriteLine($"测试{StaticData.TimestampMilliSecondsData}转datetime");
// Console.WriteLine(StaticData.TimestampSecondsData.MillisecondsToDateTime());

#endregion

#region mapper对象映射

// Console.WriteLine("测试mapper映射");
// var userSO = MyMapper.UserToUserSO(StaticData.DemoUser);

#endregion


#region json转换

var opt = new JsonSerializerOptions
{
    // 空格
    WriteIndented = true,
    // 宽松转义规则,虽然不规范,但中文会正常打印出来. 否则中文会变成unicode字符,例如'蓝'-'\u84DD'
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

    // 下面的不常用
    // 默认不包含字段
    IncludeFields = true,
    // 允许存在注释,例如 "a":1, // a是id
    ReadCommentHandling = JsonCommentHandling.Skip,
    // 允许结尾的逗号
    AllowTrailingCommas = true,
    // 默认大小写不敏感
    PropertyNameCaseInsensitive = true,
    // 默认允许从string中读取数字
    NumberHandling = JsonNumberHandling.AllowReadingFromString,
    // 默认驼峰,可以 UpperCaseNamingPolicy:JsonNamingPolicy 然后重写 public override string ConvertName(string name) =>name.ToUpper();来修改
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    // 对象内部驼峰 "AB":{"aB":1,"bB":1}
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
    // enum用名称,而不是数字表示
    Converters =
    {
        new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
    }
};
// 转字符串
var str = JsonSerializer.Serialize(StaticData.DemoPerson, opt);
Console.WriteLine(str);

// 转对象
var d1 = JsonSerializer.Deserialize<Person>(str);

// 直接解析取值

#region JsonNode

var jNode = JsonNode.Parse(str)!;
var name = jNode["name"]!.GetValue<string>();
Console.WriteLine(name);
// 修改
jNode["name"] = "kent";
name = jNode["name"]!.GetValue<string>();
Console.WriteLine(name);
// 移除
var jObject = jNode.AsObject();
jObject.Remove("name");

#endregion

#region JsonDocument

using var jDoc = JsonDocument.Parse(str);
name = jDoc.RootElement.GetProperty("name").Deserialize<string>();
Console.WriteLine(name);

#endregion

// Utf8JsonWriter和Utf8JsonReader
Utf8Json.TestUtf8Json();

#endregion

Console.WriteLine("done");

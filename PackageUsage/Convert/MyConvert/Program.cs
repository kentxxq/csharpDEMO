// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using DemoData;

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
    // 默认不包含字段
    IncludeFields = true
};
// 转字符串
var str = JsonSerializer.Serialize(StaticData.DemoPerson, opt);
Console.WriteLine(str);


// 直接解析取值JsonNode
var jNode = JsonNode.Parse(str)!;
var name = jNode["Name"]!.GetValue<string>();
Console.WriteLine(name);
// 修改
jNode["Name"] = "kent";
name = jNode["Name"]!.GetValue<string>();
Console.WriteLine(name);
// 移除
var jObject = jNode.AsObject();
jObject.Remove("Name");


// JsonDocument
using var jDoc = JsonDocument.Parse(str);
name = jDoc.RootElement.GetProperty("Name").Deserialize<string>();
Console.WriteLine(name);

Console.WriteLine("done");

#endregion

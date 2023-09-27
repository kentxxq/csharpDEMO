// See https://aka.ms/new-console-template for more information

using System.Text.Encodings.Web;
using System.Text.Json;
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

var str = JsonSerializer.Serialize(StaticData.DemoPerson, new JsonSerializerOptions
{
    // 空格
    WriteIndented = true,
    // 宽松转义规则,虽然不规范,但中文会正常打印出来. 否则中文会变成unicode字符,例如'蓝'-'\u84DD'
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
});

Console.WriteLine(str);

#endregion

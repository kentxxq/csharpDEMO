using System.ComponentModel.DataAnnotations;
using System.Drawing;
using DemoData.Models;

namespace DemoData;

[Display(Description = "静态数据")]
public static class StaticData
{
    [Display(Name = "密码")] public const string Password = "123456";

    [Display(Name = "字符串数据")] public const string StringData = "我是数据";

    [Display(Name = "示例User-简单", Description = "使用了Models-User")]
    public static readonly User DemoUser = new("ken", 1);

    [Display(Name = "示例Person-嵌套", Description = "使用了Models-Person")]
    public static readonly Person DemoPerson = new()
    {
        Name = "ken", Age = 1, PersonHead = new Head { Height = 50, Width = 50 }, PersonShoes = new List<Shoes>
        {
            new() { ShoesColor = Color.Blue, ShoesName = "蓝色" },
            new() { ShoesColor = Color.Red, ShoesName = "红色" }
        }
    };

    [Display(Name = "日期数据")] public static readonly DateTime DatetimeData = new(2000, 1, 2, 3, 4, 5, 678);

    [Display(Name = "timestamp数据-秒")] public static readonly long TimestampSecondsData = 1678952864;

    [Display(Name = "timestamp数据-毫秒")] public static readonly long TimestampMilliSecondsData = 1678952864041;

    [Display(Name = "列表字符串")] public static readonly List<string> ListStrings = new() { "a", "b" };
}

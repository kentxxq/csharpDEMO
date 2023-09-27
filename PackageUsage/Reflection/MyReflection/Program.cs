// See https://aka.ms/new-console-template for more information

using System.Reflection;
using DemoData;
using DemoData.Models;
using MyReflection;

Console.WriteLine($"获取class的display...");
var displayAttr = typeof(User).GetSelfDisplay();
if(displayAttr.GetAnyValueProperties().Count > 0)
{
    foreach (var property in displayAttr.GetAnyValueProperties())
    {
        Console.WriteLine($"property:{property.Name},value:{property.GetValue(displayAttr)}");
    }
}

Console.WriteLine($"获取field的display...");
displayAttr = typeof(StaticData).GetDisplayByFieldName(nameof(StaticData.DemoUser));
if(displayAttr.GetAnyValueProperties().Count > 0)
{
    foreach (var property in displayAttr.GetAnyValueProperties())
    {
        Console.WriteLine($"property:{property.Name},value:{property.GetValue(displayAttr)}");
    }
}



Console.WriteLine($"获取实例对象的字段的display...");
var qqq = StaticData.DemoUser;
var t = qqq.GetType();
displayAttr = t.GetMemberType(nameof(qqq.Name)) switch {
    MemberTypes.Field => t.GetDisplayByFieldName(nameof(qqq.Name)),
    MemberTypes.Property => t.GetDisplayByPropertyName(nameof(qqq.Name)),
    _ => throw new ArgumentException("只支持Field和Property。因为如果一个class有多个Constructor构造方法，它们的名字都会是'.ctor'。所以GetXXXFromMember(string memberName)无法实现。或者说使用起来会有歧义")
};
if(displayAttr.GetAnyValueProperties().Count > 0)
{
    foreach (var property in displayAttr.GetAnyValueProperties())
    {
        Console.WriteLine($"property:{property.Name},value:{property.GetValue(displayAttr)}");
    }
}

var pp = typeof(User).GetConstructors();
foreach (var p in pp)
{
    Console.WriteLine(p.Name);
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DemoData.Models;

/// <summary>人</summary>
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
public class Person
{
    [Display(Name = "人名", Description = "人的名字")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "性别", Description = "人的性别")]
    public Sex SexType { get; set; } = Sex.Unknown;

    [Display(Name = "生日", Description = "人的生日")]
    public DateTime Birthday { get; set; } = DateTime.Now;

    [Display(Name = "年纪", Description = "人的年纪")]
    public int Age { get; set; }

    [Display(Name = "头", Description = "人的头")]
    public Head PersonHead { get; set; } = new() { Height = 1, Width = 1 };

    [Display(Name = "鞋子", Description = "人的鞋子")]
    public List<Shoes>? PersonShoes { get; set; }
}

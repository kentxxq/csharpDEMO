using System.ComponentModel.DataAnnotations;

namespace DemoData.Models;

/// <summary>人</summary>
public class Person
{
    [Display(Name = "人名", Description = "人的名字")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "性别", Description = "人的性别")]
    public Sex SexType { get; set; }

    [Display(Name = "生日", Description = "人的生日")]
    public DateTime Birthday { get; set; }

    [Display(Name = "年纪", Description = "人的年纪")]
    public int Age { get; set; }

    [Display(Name = "头", Description = "人的头")]
    public Head PersonHead { get; set; } = null!;

    [Display(Name = "鞋子", Description = "人的鞋子")]
    public List<Shoes>? PersonShoes { get; set; }
}

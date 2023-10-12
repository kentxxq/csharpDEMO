using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace DemoData.Models;

/// <summary>鞋子</summary>
public class Shoes
{
    [Display(Name = "鞋名", Description = "鞋子的名字")]
    public string ShoesName { get; set; } = string.Empty;

    [Display(Name = "鞋颜色", Description = "鞋子的颜色")]
    public Color ShoesColor { get; set; }
}

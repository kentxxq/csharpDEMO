using System.ComponentModel.DataAnnotations;

namespace DemoData.Models;

/// <summary>头</summary>
public class Head
{
    [Display(Name = "宽", Description = "头的宽")]
    public int Width { get; set; }

    [Display(Name = "高", Description = "头的高")]
    public int Height { get; set; }
}

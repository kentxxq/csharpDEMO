using System.ComponentModel.DataAnnotations;

namespace DemoData.Models;

/// <summary>性别</summary>
public enum Sex
{
    [Display(Name = "无数据")] Unknown,
    [Display(Name = "男")] Man,
    [Display(Name = "女")] Woman
}

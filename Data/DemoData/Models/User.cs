using System.ComponentModel.DataAnnotations;

namespace DemoData.Models;

/// <summary>
/// 字段用于内部存储数据
/// 属性用于外部交互。封装的同时进行一定限制，保证安全
/// </summary>
[Display(Name = "User模型")]
public class User
{
    public User()
    {
        
    }
    public User(string nameField, int ageField)
    {
        NameField = nameField;
        AgeField = ageField;
    }

    [Display(Name = "名字字段")]
    private string NameField { get; set; }
    [Display(Name = "年龄字段")]
    private int AgeField { get; set; }

    [Display(Name = "名字属性")]
    public string Name {
        get { return NameField; }
        set { NameField = value; }
    }
    [Display(Name = "年龄属性")]
    public int Age {
        get { return AgeField; }
        set { AgeField = value; }
    }
}
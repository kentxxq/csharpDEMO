using System.Reflection;

namespace MyReflection;

public static class CheckObjectExtension
{
    public static MemberTypes GetMemberType(this Type t,string memberName)
    {
        var members = t.GetMember(memberName);
        if (members.Length > 0)
        {
            return members[0].MemberType;
        }

        throw new ArgumentException("没有这个成员");
    }
    
    #region field
    
    /// <summary>
    /// 拿到有值的Field
    /// </summary>
    /// <param name="o"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<FieldInfo> GetAnyValueFields<T>(this T o)
    {
        List<FieldInfo> anyValueFields = new();
        if (o == null)
        {
            return anyValueFields;
        }
        
        var t = o.GetType();
        var fields = t.GetFields();
        foreach (var field in fields)
        {
            try
            {
                if (field.GetValue(o) != null && field.GetValue(o)?.ToString() != "")
                {
                    anyValueFields.Add(field);
                }
            }
            catch (Exception)
            {
                // 如果如何获取，那么算是没有值
            }
        }

        return anyValueFields;
    }

    /// <summary>
    /// 检查fields存在null或""空字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool AnyEmptyOrNullField<T>(this T o)
    {
        if (o is null)
        {
            return true;
        }

        var t = o.GetType();
        return o.GetAnyValueFields().Count != t.GetFields().Length;
    }

    /// <summary>
    /// 检查fields是否有一个不为null或""空字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool AnyFieldValue<T>(this T o)
    {
        if (o == null)
        {
            return false;
        }

        return o.GetAnyValueFields().Count > 0;
    }

    #endregion

    #region property
    
    
    /// <summary>
    /// 拿到有值的Property
    /// </summary>
    /// <param name="o"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static List<PropertyInfo> GetAnyValueProperties<T>(this T o)
    {
        List<PropertyInfo> anyValueProperties = new();
        if (o == null)
        {
            return anyValueProperties;
        }
        
        var t = o.GetType();
        var properties = t.GetProperties();
        foreach (var property in properties)
        {
            try
            {
                if (property.GetValue(o) != null && property.GetValue(o)?.ToString() != "")
                {
                    anyValueProperties.Add(property);
                }
            }
            catch (Exception)
            {
                // 如果如何获取，那么算是没有值
            }
        }

        return anyValueProperties;
    }

    /// <summary>
    /// 检查Property存在null或""空字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool AnyEmptyOrNullProperty<T>(this T o)
    {
        if (o is null)
        {
            return true;
        }

        var t = o.GetType();
        return o.GetAnyValueProperties().Count != t.GetProperties().Length;
    }

    /// <summary>
    /// 检查Property是否有一个不为null或""空字符串
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="o"></param>
    /// <returns></returns>
    public static bool AnyPropertyValue<T>(this T o)
    {
        if (o == null)
        {
            return false;
        }

        return o.GetAnyValueProperties().Count > 0;
    }

    #endregion
}
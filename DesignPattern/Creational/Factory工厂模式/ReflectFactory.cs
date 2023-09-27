namespace Factory;

public static class ReflectFactory
{
    public static IPhone? CreatePhone(string typeName)
    {
        var type = Type.GetType(typeName, true, true);
        var phone = type?.Assembly.CreateInstance(typeName) as IPhone;
        return phone;
    }
}
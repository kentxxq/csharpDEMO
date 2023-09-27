namespace Factory;

public interface IBigPhoneFactory
{
    void CreateStandardPhone();
    void CreateProPhone();
    void CreateMaxPhone();
}

public class XiaomiFactory : IBigPhoneFactory
{
    public void CreateStandardPhone()
    {
        Console.WriteLine("xiaomi StandardPhone");
    }

    public void CreateProPhone()
    {
        Console.WriteLine("xiaomi ProPhone");
    }

    public void CreateMaxPhone()
    {
        Console.WriteLine("xiaomi MaxPhone");
    }
}

public class AppleFactory : IBigPhoneFactory
{
    public void CreateStandardPhone()
    {
        Console.WriteLine("apple StandardPhone");
    }

    public void CreateProPhone()
    {
        Console.WriteLine("apple ProPhone");
    }

    public void CreateMaxPhone()
    {
        Console.WriteLine("apple MaxPhone");
    }
}
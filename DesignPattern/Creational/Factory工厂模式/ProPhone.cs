namespace Factory;

public class ProPhone : IPhone
{
    public void Call()
    {
        Console.WriteLine("ProPhone call...");
    }

    public void Play()
    {
        Console.WriteLine("ProPhone play...");
    }
}
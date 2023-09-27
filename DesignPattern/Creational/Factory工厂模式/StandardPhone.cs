namespace Factory;

public class StandardPhone : IPhone
{
    public void Call()
    {
        Console.WriteLine("StandardPhone call...");
    }

    public void Play()
    {
        Console.WriteLine("StandardPhone play...");
    }
}
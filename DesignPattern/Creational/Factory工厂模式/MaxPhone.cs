namespace Factory;

public class MaxPhone : IPhone
{
    public void Call()
    {
        Console.WriteLine("MaxPhone call...");
    }

    public void Play()
    {
        Console.WriteLine("MaxPhone play...");
    }
}
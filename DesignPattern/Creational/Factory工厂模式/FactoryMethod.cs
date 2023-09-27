namespace Factory;

public interface ISpecialFactory
{
    IPhone CreatePhone();
}

public class RandomSpecialFactory : ISpecialFactory
{
    public IPhone CreatePhone()
    {
        var enums = Enum.GetValues(typeof(PhoneEnum));
        var i = new Random();
        var phoneEnum = (PhoneEnum)i.Next(enums.Length - 1);
        var randomPhone = SimpleFactory.CreatePhone(phoneEnum);
        return randomPhone;
    }
}

public class MaxSpecialFactory : ISpecialFactory
{
    public IPhone CreatePhone()
    {
        return new MaxPhone();
    }
}

public class ProSpecialFactory : ISpecialFactory
{
    public IPhone CreatePhone()
    {
        return new ProPhone();
    }
}

public class StandardSpecialFactory : ISpecialFactory
{
    public IPhone CreatePhone()
    {
        return new StandardPhone();
    }
}
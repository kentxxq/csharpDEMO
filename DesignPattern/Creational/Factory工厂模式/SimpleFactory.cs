namespace Factory;

public static class SimpleFactory
{
    public static IPhone CreatePhone(PhoneEnum phoneEnum)
    {
        return phoneEnum switch {
            PhoneEnum.Standard => new StandardPhone(),
            PhoneEnum.Pro => new ProPhone(),
            PhoneEnum.Max => new MaxPhone(),
            _ => throw new ArgumentOutOfRangeException(nameof(phoneEnum), phoneEnum, "错误的phone类型")
        };
    }
}
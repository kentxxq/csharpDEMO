namespace Singleton单例模式;

/// <summary>
/// 懒汉模式，切线程不安全
/// </summary>
public sealed class BadSingleton
{
    private static BadSingleton? _instance;

    private BadSingleton()
    {
    }

    public static BadSingleton Instance => _instance ??= new BadSingleton();
}
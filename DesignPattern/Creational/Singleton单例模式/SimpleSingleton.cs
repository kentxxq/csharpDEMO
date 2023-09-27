namespace Singleton单例模式;

/// <summary>
/// 饿汉模式
/// </summary>
public class SimpleSingleton
{
    private static SimpleSingleton? _instance;
    private static readonly object Padlock = new();

    private SimpleSingleton()
    {
    }

    public static SimpleSingleton Instance {
        get {
            lock (Padlock)
            {
                return _instance ??= new SimpleSingleton();
            }
        }
    }
}
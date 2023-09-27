namespace Singleton单例模式;

public class MySingleton
{
    /// <summary>
    /// 禁止实例化
    /// </summary>
    private MySingleton()
    {
    }

    // ReSharper disable once InconsistentNaming
    private static MySingleton _MySingletonInstance => new();

    /// <summary>
    /// 静态成员虽然是全局共享的。不同线程调用同一个静态方法时，他们不会共享静态方法内部创建的参数
    /// 这就会导致线程不安全
    /// </summary>
    public static MySingleton MySingletonInstance => _MySingletonInstance;
}
namespace Singleton单例模式;

/// <summary>
/// 懒汉模式
/// </summary>
public sealed class LazySingleton
{
    private static readonly Lazy<LazySingleton> Lazy = new(() => new LazySingleton());

    private LazySingleton()
    {
    }

    /// <summary>
    /// 一个类有静态构造函数，就会不会被c#编译器标记为beforefieldinit
    /// 被标记为beforefieldinit，说明初始化可能会在调用前。
    /// 而我们不被标记成被标记为beforefieldinit，就可以确保一定在调用后才初始化。也就是懒汉模式
    /// </summary>
    public static LazySingleton Instance => Lazy.Value;
}
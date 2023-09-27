// See https://aka.ms/new-console-template for more information

using Singleton单例模式;

Thread t;
// 线程不安全，不同的hashcode
for (var i = 0; i < 2; i++)
{
    t = new Thread(() =>
    {
        var code = BadSingleton.Instance.GetHashCode();
        Console.WriteLine($"BadSingleton hashcode:{code}");
    });
    t.Start();
}

// 线程安全，一样的hashcode
for (var i = 0; i < 2; i++)
{
    t = new Thread(() =>
    {
        var code = SimpleSingleton.Instance.GetHashCode();
        Console.WriteLine($"SimpleSingleton hashcode:{code}");
    });
    t.Start();
}

// 线程安全，一样的hashcode
for (var i = 0; i < 2; i++)
{
    t = new Thread(() =>
    {
        var code = LazySingleton.Instance.GetHashCode();
        Console.WriteLine($"LazySingleton hashcode:{code}");
    });
    t.Start();
}

// 线程不安全，不同的hashcode
for (var i = 0; i < 2; i++)
{
    t = new Thread(() =>
    {
        var code = MySingleton.MySingletonInstance.GetHashCode();
        Console.WriteLine($"MySingleton hashcode:{code}");
    });
    t.Start();
}
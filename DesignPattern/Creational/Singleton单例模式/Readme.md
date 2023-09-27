通常在开发过程中通过DI注入来管理声明周期，所以不一定经常会用到了。下面只记录了线程安全的方法：

1. `BadSingleton`是懒汉模式，且线程不安全
2. `SimpleSingletonPerson`是饿汉模式
3. `LazySingleton`是懒汉模式

[参考链接](https://csharpindepth.com/articles/singleton)
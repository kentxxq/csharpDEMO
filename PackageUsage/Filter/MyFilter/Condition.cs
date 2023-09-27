namespace MyFilter;

public static class Condition
{
    /// <summary>
    /// 传入bool来决定是否使用指定func过滤数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="branch"></param>
    /// <returns></returns>
    public static IEnumerable<T> If<T>(this IEnumerable<T> source, bool condition,
        Func<IEnumerable<T>, IEnumerable<T>> branch)
    {
        return condition ? branch(source) : source;
    }
}
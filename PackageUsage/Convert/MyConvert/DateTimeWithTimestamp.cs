namespace MyConvert;

public static class DateTimeWithTimestamp
{
    /// <summary>datetime转timestamp毫秒</summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToTimestampMilliseconds(this DateTime dateTime)
    {
        // return (dateTime.ToUniversalTime().Ticks - 621355968000000000) / 10000;
        return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }

    /// <summary>datetime转timestamp秒</summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ToTimestampSeconds(this DateTime dateTime)
    {
        return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    /// <summary>timestamp毫秒转datetime</summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DateTime MillisecondsToDateTime(this long data)
    {
        var initDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var result = initDateTime.AddMilliseconds(data).ToLocalTime();
        return result;
    }

    /// <summary>timestamp秒转datetime</summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static DateTime SecondsToDateTime(this long data)
    {
        var initDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        var result = initDateTime.AddSeconds(data).ToLocalTime();
        return result;
    }
}

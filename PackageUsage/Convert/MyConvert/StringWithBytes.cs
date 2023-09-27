using System.Security.Cryptography;
using System.Text;

namespace MyConvert;

public static class StringWithBytes
{
    /// <summary>
    /// 字符串转字节数组
    /// </summary>
    /// <returns></returns>
    public static byte[] StringToBytes(string data)
    {
        var bytes = Encoding.UTF8.GetBytes(data);
        return bytes;
    }
    
    /// <summary>
    /// 字节数组转string
    /// </summary>
    /// <returns></returns>
    public static string BytesToString(byte[] data)
    {
        var s = Encoding.UTF8.GetString(data);
        return s;
    }
    
    /// <summary>
    /// 输出打印Bytes
    /// aes生成的key是随机的256位二进制数字，也肯定不是utf8字符集了。
    /// Convert是从 基础类型 转换到另一个 基础类型
    /// bitConvert是将 基础类型 转换成 字节数组，或者将 字节数组 转换成 基础类型
    /// 那么我们这里应该用bitConvert，因为我们在将数组转换成基础类型
    /// 一个byte的最大值是256，BitConverter.ToString是将每一个byte转换为16进制，然后变成string显示。如果第一个byte是十进制的256，那么就会打印出FF
    /// Convert.toHexString也可以打印出来，，一样是16进制，都是用2个字符来表示。输出的结果会是一致的(只是在字符串中没有中杠-)
    /// </summary>
    public static void PrintBytes()
    {
        var aes = Aes.Create();
        var keyString = BitConverter.ToString(aes.Key);
        var ivString = BitConverter.ToString(aes.IV);
        Console.WriteLine($"BitConverter.ToString  aesKey:{keyString},aesIV:{ivString}");
        
        var keyString2 = Convert.ToHexString(aes.Key);
        var ivString2 = Convert.ToHexString(aes.IV);
        Console.WriteLine($"Convert.ToHexString  aesKey:{keyString},aesIV:{ivString}");
    }
}
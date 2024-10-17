using System.Security.Cryptography;
using System.Text;

namespace MyEncrypt;

public static class Encode
{
    /// <summary>
    /// 计算文本的sha256
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string Sha256(this string text)
    {
        var textBytes = Encoding.UTF8.GetBytes(text);
        var textHash = SHA256.HashData(textBytes);
        var result = BitConverter.ToString(textHash).Replace("-", "").ToLower();
        return result;
    }
}

using System.Security.Cryptography;
using System.Text;

namespace MyEncrypt;

public static class KAes
{
    /// <summary>
    /// 密码sha256作为key，md5作为iv。然后采用aes方式加密data
    /// </summary>
    /// <param name="data">需要加密的数据</param>
    /// <param name="password">密码</param>
    /// <returns></returns>
    public static byte[] EncryptBytes(byte[] data,string password)
    {
        var aes = Aes.Create();
        
        // sha256的长度刚好256位，可以作为Key
        var keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        Console.WriteLine($"password的sha256为:{BitConverter.ToString(keyBytes)}");
        
        // md5的长度刚好128位，可以作为IV
        var ivBytes = MD5.HashData(Encoding.UTF8.GetBytes(password));
        Console.WriteLine($"password的md5为:{BitConverter.ToString(ivBytes)}");

        var aesEncryptor = aes.CreateEncryptor(keyBytes, ivBytes);

        var transformFinalBlock = aesEncryptor.TransformFinalBlock(data, 0, data.Length);
        return transformFinalBlock;
    }
}
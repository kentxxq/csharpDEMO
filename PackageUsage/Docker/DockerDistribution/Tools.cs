using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DockerDistribution;

public class Tools
{
    /// <summary>
    /// json序列化配置
    /// </summary>
    public static JsonSerializerOptions MyJsonSerializerOptions = new()
    {
        // 空格
        WriteIndented = true,
        // 宽松转义规则,虽然不规范,但中文会正常打印出来. 否则中文会变成unicode字符,例如'蓝'-'\u84DD'
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,

        // 下面的不常用
        // 默认不包含字段
        IncludeFields = true,
        // 允许存在注释,例如 "a":1, // a是id
        ReadCommentHandling = JsonCommentHandling.Skip,
        // 允许结尾的逗号
        AllowTrailingCommas = true,
        // 默认大小写不敏感
        PropertyNameCaseInsensitive = true,
        // 默认允许从string中读取数字
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        // 默认驼峰. JsonNamingPolicy.SnakeCaseLower 是小写下划线分割
        // https://learn.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json/customize-properties?pivots=dotnet-8-0#use-a-built-in-naming-policy
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        // 对象内部驼峰 "AB":{"aB":1,"bB":1}
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        // enum用名称,而不是数字表示
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        },
        // 从源生成中获取类型转换信息
        // TypeInfoResolver = JsonContext.Default
    };
}

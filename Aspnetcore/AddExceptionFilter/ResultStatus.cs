using System.ComponentModel.DataAnnotations;

namespace AddExceptionFilter;

/// <summary>
///     统一返回模型的枚举code
/// </summary>
public enum ResultStatus
{
    /// <summary>
    ///     成功
    /// </summary>
    [Display(Name = "成功", Description = "请求成功返回")]
    Success = 20000,

    /// <summary>
    ///     失败
    /// </summary>
    [Display(Name = "失败", Description = "内部服务异常")]
    Error = 50000
}

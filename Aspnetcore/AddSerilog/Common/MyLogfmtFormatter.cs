using Serilog.Formatting;
using Serilog.Logfmt;

namespace AddSerilog.Common;

public static class MyLogfmtFormatter
{
    public static ITextFormatter Formatter {get;} = new LogfmtFormatter(opt =>
    {
        // 双引号用单引号代替
        opt.OnDoubleQuotes(q => q.ConvertToSingle());
        // 打印所有的额外sourceContext
        opt.IncludeAllProperties();
        // 保持原有的大小写模式。默认会变成下划线分割，例如XiaoLi变成xiao_li
        opt.PreserveCase();
    });
}

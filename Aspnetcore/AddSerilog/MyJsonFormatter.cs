using Serilog.Formatting;
using Serilog.Formatting.Json;

namespace AddSerilog;

public static class MyJsonFormatter 
{ 
    public static ITextFormatter Formatter {get;} = new JsonFormatter(renderMessage:true); 
} 

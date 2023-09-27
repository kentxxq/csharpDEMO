using System.Net.Mime;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace PrintRequest;

public class PrintRequestMiddleware:IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        Dictionary<string, object> data = new();


        var headers = context.Request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString());
        if (headers.Count > 0)
        {
            data.Add("Headers",headers);
            
            // result += "Headers: \n";
            //
            // foreach (var (key, value) in headers)
            // {
            //     result += $"{key}: {value} \n";
            // }
        }
        
        data.Add("method",context.Request.Method);
        data.Add("schema",context.Request.Scheme);
        data.Add("host",context.Request.Host);
        data.Add("path",context.Request.Path);
        data.Add("queryString",(string.IsNullOrEmpty(context.Request.QueryString.Value)?"无查询参数":context.Request.QueryString.Value));
        
        // result += $"请求method:{context.Request.Method} \n";
        // result += $"请求schema:{context.Request.Scheme} \n";
        // result += $"请求host:{context.Request.Host} \n";
        // result += $"请求path:{context.Request.Path} \n";
        // result += $"请求queryString:{(string.IsNullOrEmpty(context.Request.QueryString.Value)?"无查询参数":context.Request.QueryString.Value)} \n";
        
        
        using (var reader = new StreamReader(context.Request.Body))
        {
            var requestBody = await reader.ReadToEndAsync();

            if (!string.IsNullOrEmpty(requestBody))
            {
                data.Add("body",requestBody);
                // result += $"请求body:{requestBody} \n";
            }
        }
        
        await context.Response.WriteAsJsonAsync(data,new JsonSerializerOptions {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
    }
}
using log4net;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace FirstWebApi.Middleware
{
    public class HeaderMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog log = LogManager.GetLogger(typeof(HeaderMiddleware));

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;

            // Extract headers and connection info
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            var os = GetOsFromUserAgent(userAgent);
            var path = request.Path.ToString();
            var http = request.Method;
            var statusCode = context.Response.StatusCode.ToString();

            // Amount of time take to respond
            var stopwatch = Stopwatch.StartNew();
            var duration = stopwatch.ElapsedMilliseconds.ToString();

            LogicalThreadContext.Properties["ip"] = ip ?? "Unknown";
            LogicalThreadContext.Properties["os"] = os ?? "Unknown";
            LogicalThreadContext.Properties["userAgent"] = userAgent ?? "Unknown";
            LogicalThreadContext.Properties["path"] = path ?? "Unknown";
            LogicalThreadContext.Properties["http"] = http ?? "Unknown";
            LogicalThreadContext.Properties["statusCode"] = statusCode ?? "Unknown";
            LogicalThreadContext.Properties["duration"] = duration ?? "Unknown";

            //log.Info($@"[HTTP={request.Method}] [PATH={request.Path}] [IP={ipAddress}] [UA={userAgent}] [OS={os}]");
            try
            {
                await _next(context);
            }
            finally
            {
                //ThreadContext.Properties.Remove("ip");
                //ThreadContext.Properties.Remove("os");
                //ThreadContext.Properties.Remove("userAgent");
                //ThreadContext.Properties.Remove("path");
                //ThreadContext.Properties.Remove("http");
                //ThreadContext.Properties.Clear();

                stopwatch.Stop();

                LogicalThreadContext.Properties.Clear();
            }

        }

        private string GetOsFromUserAgent(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent)) return "Unknown";

            if (userAgent.Contains("Windows", StringComparison.OrdinalIgnoreCase)) return "Windows";
            if (userAgent.Contains("Mac", StringComparison.OrdinalIgnoreCase)) return "MacOS";
            if (userAgent.Contains("Linux", StringComparison.OrdinalIgnoreCase)) return "Linux";
            if (userAgent.Contains("Android", StringComparison.OrdinalIgnoreCase)) return "Android";
            if (userAgent.Contains("iPhone", StringComparison.OrdinalIgnoreCase)) return "iOS";

            return "Other";
        }
    }
}

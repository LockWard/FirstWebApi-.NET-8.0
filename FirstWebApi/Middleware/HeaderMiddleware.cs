using log4net;

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
            var ipAddress = context.Connection.RemoteIpAddress?.ToString();
            var userAgent = request.Headers["User-Agent"].ToString();
            var os = GetOsFromUserAgent(userAgent);

            log.Info($@"Incoming request: Method: {request.Method} Path: {request.Path} IP: {ipAddress} User-Agent: {userAgent} OS: {os}");

            await _next(context);
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

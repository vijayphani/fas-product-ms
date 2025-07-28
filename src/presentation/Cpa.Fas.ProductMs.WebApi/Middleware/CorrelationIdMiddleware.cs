namespace Cpa.Fas.ProductMs.WebApi.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;
        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(CorrelationIdHeaderName))
            {
                context.Request.Headers[CorrelationIdHeaderName] = Guid.NewGuid().ToString();
            }
            var correlationId = context.Request.Headers[CorrelationIdHeaderName].ToString();
            context.Items[CorrelationIdHeaderName] = correlationId;
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeaderName] = correlationId;
                return Task.CompletedTask;
            });
            _logger.LogInformation("Incoming request with Correlation Id: {CorrelationId}", correlationId);
            await _next(context);
            _logger.LogInformation("Outgoing response with Correlation Id: {CorrelationId}", correlationId);
        }
    }
}

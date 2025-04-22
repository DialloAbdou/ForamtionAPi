
namespace Mod3ASPNET.Filters
{
    public class LogginFilter : IEndpointFilter
    {
        private readonly ILogger<LogginFilter> _logger;

        public LogginFilter(ILogger<LogginFilter> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            _logger.LogInformation( $"Avant  l'encement de EndPoint File : {context.HttpContext.Request.Path}");
            var result = await next(context);
            _logger.LogInformation($"Après  l'encement de EndPoint File :{context.HttpContext.Request.Path}");
            return result;
        }
    }
}

namespace Ecoplaza.Middleware
{
    public class SubdomainMiddleware
    {
        private readonly RequestDelegate _next;

        public SubdomainMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, SubdomainService subdomainService)
        {
            // Инициализация поддомена на каждом запросе
            subdomainService.Initialize(context);

            // Передаем запрос дальше по конвейеру
            await _next(context);
        }
    }

}

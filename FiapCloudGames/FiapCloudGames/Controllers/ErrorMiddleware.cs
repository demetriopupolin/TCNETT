namespace FiapCloudGamesApi.Controllers
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Threading.Tasks;

    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro: {e.Message}");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Erro interno.");
            }
        }
    }

}

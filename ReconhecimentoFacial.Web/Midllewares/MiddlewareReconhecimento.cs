using ReconhecimentoFacial.Lib.MinhasExceptions;

namespace ReconhecimentoFacial.Web.Midllewares
{
    public class MiddlewareReconhecimento
    {
        private readonly RequestDelegate _next;
        public MiddlewareReconhecimento(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidacaoDeDados ex)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(ex.Message);
            }
        }
    }
}
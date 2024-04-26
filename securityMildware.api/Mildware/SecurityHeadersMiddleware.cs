using Microsoft.Extensions.Primitives;
using SecurityMiddleware.api.Interface;

namespace SecurityMiddleware.api
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ISecureDomainPolicy _secureDomainPolicy;

        public SecurityHeadersMiddleware(RequestDelegate next, ISecureDomainPolicy secureDomainPolicy)
        {
            _next = next;
            _secureDomainPolicy = secureDomainPolicy;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Agregar los encabezados de seguridad
            //context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("Referrer-Policy", "no-referrer");
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
            context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

            context.Response.Headers.Add("Expect-CT", new StringValues("max-age=86400, enforce, report-uri=\"https://dfgsdffdhfhdfghdfgh.report-uri.com/r/d/csp/enforce\""));
            context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            context.Response.Headers.Add("Pragma", "no-cache");
            context.Response.Headers.Add("Expires", "0");
            context.Response.Headers.Remove("Server");
            context.Response.Headers.Remove("X-Powered-By");
            context.Response.Headers.Remove("X-AspNet-Version");

            context.Response.Headers.Add("Permissions-Policy", new StringValues(
                "accelerometer=(), " + // () deniega todo
                "autoplay=(), " +
                "camera=self, " +
                "document-domain=self, " + //self  significa que el recurso o la acción solo puede ser utilizada por el mismo origen que la página actua
                "encrypted-media=self, " +
                "fullscreen=*, " + // Si quieres permitir pantalla completa, si no, usa ()
                "gyroscope=(), " +
                "magnetometer=(), " +
                "microphone=self, " +
                "midi=self, " +
                "payment=(), " +
                "picture-in-picture=(), " +
                "publickey-credentials-get=(), " +
                "sync-xhr=(), " +
                "usb=self, " +
                "xr-spatial-tracking=()"
            ));

            context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                $"{_secureDomainPolicy.ScriptSrc}{_secureDomainPolicy.StyleSrc}{_secureDomainPolicy.ImgSrc}" +
                $"{_secureDomainPolicy.FontSrc}{_secureDomainPolicy.ConnectSrc}" +
                $"{_secureDomainPolicy.FormAction}{_secureDomainPolicy.FrameSrc}"
            ));

            context.Response.GetTypedHeaders().CacheControl =
                new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(10)
                };

            await _next(context);
        }
    }
}

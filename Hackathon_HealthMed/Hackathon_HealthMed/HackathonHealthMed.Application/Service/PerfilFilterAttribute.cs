using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HackathonHealthMed.Application.Service
{
    public class PerfilFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly EPerfil _perfilEsperado;

        public PerfilFilterAttribute(EPerfil perfilEsperado)
        {
            _perfilEsperado = perfilEsperado;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Token"].ToString();
            var loginService = (ILoginService)context.HttpContext.RequestServices.GetService(typeof(ILoginService));

            if (loginService == null)
            {
                context.Result = new StatusCodeResult(500);
                return;
            }

            var user = await loginService.IdentityUserAsync(token);

            if (user.perfil != _perfilEsperado)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }

}

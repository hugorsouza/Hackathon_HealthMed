using HackathonHealthMed.Application.Interfaces;
using HackathonHealthMed.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace HackathonHealthMed.Application.Service
{
    public class PerfilFilterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly EPerfil _perfilEsperado;

        public PerfilFilterAttribute(EPerfil perfilEsperado)
        {
            _perfilEsperado = perfilEsperado;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var perfilClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "perfil");

                if (perfilClaim == null || !Enum.TryParse(perfilClaim.Value, out EPerfil perfilUsuario))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                if (perfilUsuario != _perfilRequerido)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }

}

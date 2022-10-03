using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Http;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.WebSupport.Utilities.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, IJwtUtils jwtUtils)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        string mAccount = jwtUtils.ValidateJwtToken(token);
        if (mAccount != null)
        {
            // attach account to context on successful jwt validation
            httpContext.Items["Account"] = AccountUtility.GetAccount(mAccount);
        }

        await _next(httpContext);
    }
}
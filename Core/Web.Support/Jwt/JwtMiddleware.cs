using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    
    [CLSCompliant(false)]
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    [CLSCompliant(false)]
    public async Task Invoke(HttpContext httpContext, IJwtUtility jwtUtils)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if(token != null)
        {
            string mAccount = jwtUtils.ValidateJwtToken(token);
            if (mAccount != null)
            {
                // attach account to context on successful jwt validation
                httpContext.Items["AccountProfile"] = AccountUtility.GetAccount(mAccount);
            }
        }
        await _next(httpContext);
    }
}
using GrowthWare.Framework;
using GrowthWare.Web.Support.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Logger m_Logger = Logger.Instance();
    
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
        try
        {
            await _next(httpContext);            
        }
        catch (System.Exception ex)
        {
            m_Logger.Error(ex);
            throw;
        }
    }
}
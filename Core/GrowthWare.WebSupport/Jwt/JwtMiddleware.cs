using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Services;

namespace GrowthWare.WebSupport.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    private MAccountProfile m_AnonymousProfile = null;
    
    [CLSCompliant(false)]
    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    [CLSCompliant(false)]
    public async Task Invoke(HttpContext httpContext, IJwtUtils jwtUtils, IAccountService accountService, IClientChoicesService clientChoicesService)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        string mAccount = jwtUtils.ValidateJwtToken(token);
        if (mAccount != null)
        {
            // attach account to context on successful jwt validation
            httpContext.Items["AccountProfile"] = accountService.GetAccount(mAccount);
            MClientChoicesState mClientChoicesState = clientChoicesService.GetClientChoicesState(mAccount);
            httpContext.Items["ClientChoicesState"] = mClientChoicesState;
        }
        else
        {
            if(this.m_AnonymousProfile == null)
            {
                this.m_AnonymousProfile = accountService.GetAccount("Anonymous");
            }           
            httpContext.Items["AccountProfile"] = this.m_AnonymousProfile;
        }

        await _next(httpContext);
    }
}
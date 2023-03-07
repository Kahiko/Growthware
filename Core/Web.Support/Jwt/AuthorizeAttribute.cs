using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace GrowthWare.Web.Support.Jwt;

[CLSCompliant(false)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    readonly string m_Action = string.Empty;
    public AuthorizeAttribute(string action)
    {
        if(action != null)
        {
            this.m_Action = action;
        }
    }

    public AuthorizeAttribute() { }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var mAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (mAllowAnonymous)
        {
            return;
        }

        // authorization
        MAccountProfile mAccount = (MAccountProfile)context.HttpContext.Items["AccountProfile"];
        if (!String.IsNullOrEmpty(this.m_Action))
        {
            MFunctionProfile mFunction = FunctionUtility.GetProfile(this.m_Action);
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunction, mAccount);
            context.HttpContext.Items["SecurityInfo"] = mSecurityInfo;
            context.HttpContext.Items["Function"] = mFunction;
            if (mAccount == null || !mSecurityInfo.MayView)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }

    }
}
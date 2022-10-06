using GrowthWare.Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace GrowthWare.WebSupport.Utilities.Jwt;

[CLSCompliant(false)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    readonly string m_Action = string.Empty;
    public AuthorizeAttribute(string action)
    {
        this.m_Action = action;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
        {
            return;
        }

        // authorization
        MAccountProfile mAccount = (MAccountProfile)context.HttpContext.Items["Account"];
        string mAction = (string)context.HttpContext.Items["Action"];
        MFunctionProfile mFunction = FunctionUtility.GetProfile(mAction, context.HttpContext);
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
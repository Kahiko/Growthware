using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthWare.Web.Support.Jwt;

[CLSCompliant(false)]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
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

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // skip authorization if action is decorated with [AllowAnonymous] attribute
        var mAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (mAllowAnonymous)
        {
            return;
        }

        // authorization
        MAccountProfile mAccount = await AccountUtility.CurrentProfile();
        // var mAccount = (MAccountProfile)context.HttpContext.Items["AccountProfile"];
        if (!String.IsNullOrEmpty(this.m_Action))
        {
            MFunctionProfile mFunction = await FunctionUtility.GetProfile(this.m_Action);
            MSecurityInfo mSecurityInfo = new MSecurityInfo(mFunction, mAccount);
            context.HttpContext.Items["SecurityInfo"] = mSecurityInfo;
            context.HttpContext.Items["Function"] = mFunction;
            // if(mAccount.IsSystemAdmin) return;
            if (mAccount == null || (!mSecurityInfo.MayView && !mAccount.IsSystemAdmin))
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}
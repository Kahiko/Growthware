using System;
using Microsoft.AspNetCore.Mvc;
using GrowthWare.Framework.Models;

namespace GrowthWare.Framework;

[Controller]
[CLSCompliant(false)]
public abstract class BaseController : ControllerBase
{
    // returns the current authenticated account (null if not logged in)
    public MAccountProfile Account => (MAccountProfile)HttpContext.Items["AccountProfile"];
    // returns the current authenticated accounts client choices (null if not logged in)
    public MClientChoices ClientChoices => (MClientChoices)HttpContext.Items["ClientChoices"];
    // returns the current security entity (default as defined in GrowthWare.json)
    public MSecurityEntity SecurityEntity => (MSecurityEntity)HttpContext.Items["SecurityEntity"];

}
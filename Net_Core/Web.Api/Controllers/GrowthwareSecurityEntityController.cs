using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareSecurityEntityController : AbstractSecurityEntityController
{
    private readonly ILogger<GrowthwareSecurityEntityController> _logger;

    public GrowthwareSecurityEntityController(ILogger<GrowthwareSecurityEntityController> logger)
    {
        this._logger = logger;
    }

}

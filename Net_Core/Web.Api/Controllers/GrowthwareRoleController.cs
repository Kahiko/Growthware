using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareRoleController : AbstractRoleController
{
    private readonly ILogger<GrowthwareRoleController> _logger;

    public GrowthwareRoleController(ILogger<GrowthwareRoleController> logger)
    {
        this._logger = logger;
    }

}

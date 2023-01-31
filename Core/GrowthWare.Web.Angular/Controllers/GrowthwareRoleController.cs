using Microsoft.AspNetCore.Mvc;
using GrowthWare.WebSupport.BaseControllers;

namespace GrowthWare.Web.Angular.Controllers;

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

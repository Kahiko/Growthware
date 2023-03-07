using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareGroupController : AbstractGroupController
{
    private readonly ILogger<GrowthwareGroupController> _logger;

    public GrowthwareGroupController(ILogger<GrowthwareGroupController> logger)
    {
        this._logger = logger;
    }

}

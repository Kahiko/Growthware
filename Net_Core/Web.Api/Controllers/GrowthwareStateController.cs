using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareStateController : AbstractStateController
{
    private readonly ILogger<GrowthwareStateController> _logger;

    public GrowthwareStateController(ILogger<GrowthwareStateController> logger)
    {
        this._logger = logger;
    }

}

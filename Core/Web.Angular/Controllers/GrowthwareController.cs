using Microsoft.AspNetCore.Mvc;
using GrowthWare.WebSupport.BaseControllers;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAPIController : AbstractController
{
    private readonly ILogger<GrowthwareAPIController> _logger;

    public GrowthwareAPIController(ILogger<GrowthwareAPIController> logger)
    {
        this._logger = logger;
    }

}

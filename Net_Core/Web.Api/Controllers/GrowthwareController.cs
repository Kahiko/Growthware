using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

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

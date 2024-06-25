using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareMessageController : AbstractMessageController
{
    private readonly ILogger<GrowthwareMessageController> _logger;

    public GrowthwareMessageController(ILogger<GrowthwareMessageController> logger)
    {
        this._logger = logger;
    }

}

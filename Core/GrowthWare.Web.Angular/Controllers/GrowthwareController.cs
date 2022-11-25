using Microsoft.AspNetCore.Mvc;
using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Services;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAPIController : AbstractController
{
    private readonly ILogger<GrowthwareAPIController> _logger;

    public GrowthwareAPIController(ILogger<GrowthwareAPIController> logger, IAccountService accountService)
    {
        this.m_AccountService = accountService;
        _logger = logger;
    }

}

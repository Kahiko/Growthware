using Microsoft.AspNetCore.Mvc;
using GrowthWare.WebSupport.BaseControllers;
using GrowthWare.WebSupport.Services;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAccountController : AbstractAccountController
{
    private readonly ILogger<GrowthwareAPIController> m_logger;

    public GrowthwareAccountController(ILogger<GrowthwareAPIController> logger, IAccountService accountService)
    {
        base.m_AccountService = accountService;
        this.m_logger = logger;
    }

}
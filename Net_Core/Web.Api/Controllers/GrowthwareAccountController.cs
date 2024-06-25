using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAccountController : AbstractAccountController
{
    private readonly ILogger<GrowthwareAPIController> m_logger;

    public GrowthwareAccountController(ILogger<GrowthwareAPIController> logger)
    {
        // base.m_AccountUtility = accountService;
        this.m_logger = logger;
    }

}
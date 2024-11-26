using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareFeedbackController : AbstractFeedbackController
{
    private readonly ILogger<GrowthwareAPIController> m_logger;

    public GrowthwareFeedbackController(ILogger<GrowthwareAPIController> logger)
    {
        this.m_logger = logger;
    }
}
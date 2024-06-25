using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareNameValuePairController : AbstractNameValuePairController
{
    public GrowthwareNameValuePairController()
    {
    }

}
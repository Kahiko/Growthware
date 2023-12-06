using Microsoft.AspNetCore.Mvc;
using GrowthWare.Web.Support.BaseControllers;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareNameValuePairController : AbstractNameValuePairController
{
    public GrowthwareNameValuePairController()
    {
    }

}
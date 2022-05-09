using Microsoft.AspNetCore.Mvc;
using GrowthWare.Framework.Models;
using GrowthWare.WebSupport.Utilities;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthwareAPIController : ControllerBase
{
    private readonly ILogger<GrowthwareAPIController> _logger;

    public GrowthwareAPIController(ILogger<GrowthwareAPIController> logger)
    {
        _logger = logger;
    }


    [HttpGet(Name = "GetAccount")]
    public String GetAccount(int AccountSeqId)
    {
        String mRetVal = string.Empty;
        return mRetVal;
    }

    [HttpPost("Search")]
    public String Search(MSearchCriteria searchCriteria)
    {
        String mRetVal = SearchUtility.GetSearchResults(searchCriteria);
        return mRetVal;
    }
}

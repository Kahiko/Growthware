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


    [HttpGet("GetAccountByAccount")]
    public MAccountProfile GetAccount(string account)
    {
        MAccountProfile mRetVal = AccountUtility.GetAccount(account);
        mRetVal.Password = string.Empty;
        return mRetVal;
    }

    [HttpGet("GetAccountById")]
    public MAccountProfile GetAccount(int accountSeqId)
    {
        MAccountProfile mRetVal = new MAccountProfile();
        mRetVal.Password = string.Empty;
        return mRetVal;
    }

    [HttpPost("Log")]
    public void Log(MLoggingProfile profile)
    {
        LoggingUtility.Save(profile);
    }

    [HttpPost("Search")]
    public String Search(MSearchCriteria searchCriteria)
    {
        String mRetVal = SearchUtility.GetSearchResults(searchCriteria);
        return mRetVal;
    }
}

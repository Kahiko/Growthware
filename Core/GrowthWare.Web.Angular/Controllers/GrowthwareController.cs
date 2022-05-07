using System.Data;
using Microsoft.AspNetCore.Mvc;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Angular.Controllers;

[ApiController]
[Route("[controller]")]
public class GrowthWareController : ControllerBase
{
    [HttpGet(Name = "GetAccount")]
    public MAccountProfile GetAccount(string accountName) {
        MAccountProfile mRetVal = new MAccountProfile();
        return mRetVal;
    }

    [HttpGet(Name = "GetClientChoices")]
    public void GetClientChoices(string accountName) {

    }

    [HttpGet(Name = "GetSearchResults")]
    public string GetSearchResults(MSearchCriteria searchCriteria) {
        String mRetVal = string.Empty;
        return mRetVal;
    }
    
}
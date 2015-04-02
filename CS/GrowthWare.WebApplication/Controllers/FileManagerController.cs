using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class FileManagerController : ApiController
    {
        [HttpPost()]
        public IHttpActionResult GetDirectoryLinks(RequestDirectoryLinksInfo requestDirectoryInfo) 
        {
            string mRetVal = FileUtility.GetDirectoryLinks(requestDirectoryInfo.CurrentDirectoryString, requestDirectoryInfo.FunctionSeqId);
            return Ok(mRetVal);
        }
    }

    public class RequestDirectoryLinksInfo
    {
        public string CurrentDirectoryString { get; set; }
        public int FunctionSeqId { get; set; }
    }
}

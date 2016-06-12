using GrowthWare.WebSupport;
using GrowthWare.WebSupport.Base;
using System;

namespace GrowthWare.WebApplication.Functions.System.Errors
{
    public partial class UnknownAction : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception mEx = GWWebHelper.ExceptionError;
            clientMsg.InnerHtml = mEx.Message.ToString();
        }
    }
}
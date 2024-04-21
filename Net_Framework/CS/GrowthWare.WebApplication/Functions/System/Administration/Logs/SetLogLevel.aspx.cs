using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.WebSupport.Base;
using System;
using System.Web.Services;

namespace GrowthWare.WebApplication.Functions.System.Administration.Logs
{
    public partial class SetLogLevel : BaseWebpage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Logger mLog = Logger.Instance();
            dropLogLevel.SelectedIndex = mLog.CurrentLogLevel;
        }

        [WebMethod(CacheDuration = 0, EnableSession = false)]
        public static void InvokeSetLogLevel(int logLevel)
        {
            Logger mLog = Logger.Instance();
            switch (logLevel)
            {
                case 0:
                    mLog.SetThreshold(LogPriority.Debug);
                    break;
                case 1:
                    mLog.SetThreshold(LogPriority.Info);
                    break;
                case 2:
                    mLog.SetThreshold(LogPriority.Warn);
                    break;
                case 3:
                    mLog.SetThreshold(LogPriority.Error);
                    break;
                case 4:
                    mLog.SetThreshold(LogPriority.Fatal);
                    break;
                default:
                    mLog.SetThreshold(LogPriority.Error);
                    break;
            }
        }
    }
}
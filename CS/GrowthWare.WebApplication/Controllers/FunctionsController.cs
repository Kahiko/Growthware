using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace GrowthWare.WebApplication.Controllers
{
    public class FunctionsController : ApiController
    {
        [HttpGet()]
        public Collection<FunctionInformation> GetFunctionData()
        {
            FunctionInformation mFunctionInformation = null;
            Collection<FunctionInformation> mRetVal = new Collection<FunctionInformation>();
            Collection<MFunctionProfile> mFunctions = FunctionUtility.Functions();
            string mAppName = ConfigSettings.AppName;
            if (mAppName.Length != 0)
            {
                mAppName = "/" + mAppName + "/";
            }
            foreach (MFunctionProfile mProfile in mFunctions)
            {
                if (mProfile.FunctionTypeSeqId != 2)
                {
                    mFunctionInformation = new FunctionInformation();
                    mFunctionInformation.Action = mProfile.Action;
                    mFunctionInformation.Location = mAppName + mProfile.Source;
                    mFunctionInformation.Description = mProfile.Description;
                    mFunctionInformation.LinkBehavior = mProfile.LinkBehavior;
                    mRetVal.Add(mFunctionInformation);
                }
            }

            return mRetVal;
        }

        [HttpPost()]
        public string SetSelectedSecurityEntity(int selectedSecurityEntityId)
        {
            MSecurityEntityProfile targetSEProfile = SecurityEntityUtility.GetProfile(selectedSecurityEntityId);
            MSecurityEntityProfile currentSEProfile = SecurityEntityUtility.CurrentProfile();
            MClientChoicesState mClientChoicesState = (MClientChoicesState)HttpContext.Current.Cache[MClientChoices.SessionName];
            MMessageProfile mMessageProfile = null;
            try
            {
                if (!ConfigSettings.CentralManagement)
                {
                    //SecurityEntityUtility.SetSessionSecurityEntity(targetSEProfile)
                    mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString(CultureInfo.InvariantCulture);
                    mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
                }
                else
                {
                    if (currentSEProfile.ConnectionString == targetSEProfile.ConnectionString)
                    {
                        mClientChoicesState[MClientChoices.SecurityEntityId] = targetSEProfile.Id.ToString(CultureInfo.InvariantCulture);
                        mClientChoicesState[MClientChoices.SecurityEntityName] = targetSEProfile.Name;
                    }
                    else
                    {
                        mClientChoicesState[MClientChoices.SecurityEntityId] = ConfigSettings.DefaultSecurityEntityId.ToString(CultureInfo.InvariantCulture);
                        mClientChoicesState[MClientChoices.SecurityEntityName] = "System";
                    }
                }
                ClientChoicesUtility.Save(mClientChoicesState);
                AccountUtility.RemoveInMemoryInformation(true);
                mMessageProfile = MessageUtility.GetProfile("ChangedSelectedSecurityEntity");
            }
            catch (Exception ex)
            {
                MMessageProfile myMessageProfile = new MMessageProfile();
                Logger mLog = Logger.Instance();
                mMessageProfile = MessageUtility.GetProfile("NoDataFound");
                Exception myEx = new Exception("SelectSecurityEntity:: reported an error.", ex);
                mLog.Error(myEx);
            }
            // update all of your in memory information
            return mMessageProfile.Body;
        }

    }

    public class FunctionInformation
    {
        public string Action { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int LinkBehavior { get; set; }
    }
}
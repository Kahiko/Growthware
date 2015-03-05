using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Encrypt
{
    public partial class EncryptDecrypt : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(CacheDuration = 0, EnableSession = false)]
        public static string Encrypt(string textValue)
        {
            string mRetVal = "Not Authorized";
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile("Encryption_Helper"), AccountUtility.CurrentProfile());
            if (mSecurityInfo.MayView) 
            {
                mRetVal = CryptoUtility.Encrypt(textValue.Trim(), SecurityEntityUtility.CurrentProfile().EncryptionType);
            }
            return mRetVal;
        }

        [WebMethod(CacheDuration = 0, EnableSession = false)]
        public static string Decrypt(string textValue)
        {
            string mRetVal = "Not Authorized";
            MSecurityInfo mSecurityInfo = new MSecurityInfo(FunctionUtility.GetProfile("EncryptionHelper"), AccountUtility.CurrentProfile());
            if (mSecurityInfo.MayView)
            {
                mRetVal = CryptoUtility.Decrypt(textValue.Trim(), SecurityEntityUtility.CurrentProfile().EncryptionType);
            }
            return mRetVal;
        }
    }
}
using GrowthWare.WebSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GrowthWare.WebApplication.Functions.System.Administration.Encrypt
{
    public partial class RandomNumbers : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod(EnableSession = false)]
        public static string GetRandomNumbers(int amountOfNumbers, int maxNumber, int minNumber)
        {
            String mRetVal = String.Empty;
            for (int i = 0; i < amountOfNumbers; i++)
            {
                mRetVal += GWWebHelper.GetRandomNumber(maxNumber, minNumber) + ", ";
            }
            return mRetVal;
        }
    }
}
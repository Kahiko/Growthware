using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Utilities;


namespace GrowthWare.CoreWeb
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			DateTime mStartTime = DateTime.Now;
			AccountUtility mAccountUtility = new AccountUtility();
			FunctionUtility mFunctionUtility = new FunctionUtility();

			MAccountProfile mAccountProfile = mAccountUtility.GetCurrentProfile();
			Collection<MAccountProfile> mAccountCollection = mAccountUtility.GetAccounts(mAccountProfile);
			Collection<MFunctionProfile> mFunctionCollection = mFunctionUtility.GetFunctions();
			MFunctionProfile mFunctionProfile = mFunctionUtility.GetFunction("generic_home");
			MSecurityInfo mSecurtyInfo = new MSecurityInfo(mFunctionProfile, mAccountProfile.DerivedRoles);
			//todo add code to test saving client choices
			mAccountProfile = mAccountUtility.GetProfile("Developer");
			mAccountProfile.SetRoles("Authenticated,Developer");
			mAccountUtility.Save(mAccountProfile, true, false);
			mAccountProfile.SetRoles("AlwaysLogon,Authenticated,Developer");
			mAccountUtility.Save(mAccountProfile, true, false);

			DateTime mEndTime = DateTime.Now;
			TimeSpan mTS = mEndTime.Subtract(mStartTime);
			lblStartTime.Text = mStartTime.ToString();
			lblStopTime.Text = mEndTime.ToString();
			lblDuration.Text = mTS.ToString();
		}
	}
}

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Web;
using GrowthWare.Framework.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Web.Controllers;

namespace GrowthWare.Framework.Web.Utilities
{
	/// <summary>
	/// FunctionUtility serves as the focal point for any web application needing to utiltize the GrowthWare framework.
	/// Web needs such as caching are handeled here
	/// </summary>
	public class FunctionUtility
	{
		private MSecurityEntityProfile m_SecurityEntityProfile = null;
		private BFunctions m_BFunctions = null;

		/// <summary>
		/// Constructor sets up the private fields
		/// </summary>
		public FunctionUtility() 
		{
			m_SecurityEntityProfile = SecurityEntityUtility.GetCurrentProfile();
			m_BFunctions = new BFunctions(m_SecurityEntityProfile, ConfigSettings.CentralManagement);
		}

		/// <summary>
		/// Retrieves all functions from the either the database or cache
		/// </summary>
		/// <returns>A Collection of MFunctinProfiles</returns>
		public Collection<MFunctionProfile> GetFunctions() 
		{
			String mCacheName = m_SecurityEntityProfile.Id.ToString() + "_Functions";
			Collection<MFunctionProfile> mRetVal = null;
			mRetVal = (Collection<MFunctionProfile>)(HttpContext.Current.Cache[mCacheName]);
			if (mRetVal == null) 
			{
				mRetVal = m_BFunctions.GetFunctions(m_SecurityEntityProfile.Id);
				CacheController.AddToCacheDependency(mCacheName, mRetVal);
			}
			return mRetVal;
		}

		/// <summary>
		/// Get a single function given it's action.
		/// </summary>
		/// <param name="action">String</param>
		/// <returns>MFunctionProfile</returns>
		/// <remarks>Returns null object if not found</remarks>
		public MFunctionProfile GetFunction(String action)
		{
			var mResult = from mProfile in GetFunctions()
						  where mProfile.Action.ToLower(CultureInfo.CurrentCulture) == action.ToLower(CultureInfo.CurrentCulture)
						 select mProfile;
			MFunctionProfile mRetVal = null;
			try
			{
				mRetVal = mResult.First();
			}
			catch (Exception)
			{
				mRetVal = null;
			}
			return mRetVal;

		}

		/// <summary>
		/// Get a single function given it's id.
		/// </summary>
		/// <param name="id">int or Integer</param>
		/// <returns>MFunctionProfile</returns>
		/// <remarks>Returns null object if not found</remarks>
		public MFunctionProfile GetFunction(int id) 
		{
			var mResult = from mProfile in GetFunctions()
						  where mProfile.Id  == id
						  select mProfile;
			MFunctionProfile mRetVal = null;
			try
			{
				mRetVal = mResult.First();
			}
			catch (Exception)
			{
				mRetVal = null;
			}
			return mRetVal;			
		}
	}
}

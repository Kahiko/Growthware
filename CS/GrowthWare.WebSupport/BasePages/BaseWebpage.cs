using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.WebSupport.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace GrowthWare.WebSupport.BasePages
{
    /// <summary>
    /// Class Page
    /// </summary>
    public class BaseWebpage : System.Web.UI.Page
    {
		/// <summary>
		/// Setup the name of the hidden field on the client for storing the view state key, 
		/// </summary>
		/// <remarks></remarks>

		private const string VIEW_STATE_FIELD_NAME = "__vi";
		/// <summary>
		/// Set the number of latest pages ViewState data to keep,
		///  uses a few more CPU cycles
		/// </summary>
		/// <remarks></remarks>
		private int VIEW_STATE_NUM_PAGES = ConfigSettings.ServerSideViewStatePages;

		private const string REQUEST_NUMBER = "__RequestNumber";

		/// <summary>
		/// Initializes a new instance of the <see cref="Page" /> class.
		/// </summary>
        public BaseWebpage()
		{
			
		}

		/// <summary>
		/// Overrides System.Web.UI.PageLoadPageStateFromPersistenceMedium
		/// </summary>
		/// <returns></returns>
		/// <remarks></remarks>
		protected override object LoadPageStateFromPersistenceMedium()
		{
			if (Convert.ToBoolean(ConfigSettings.ServerSideViewState)) {
				return this.LoadViewState();
			}
			return base.LoadPageStateFromPersistenceMedium();
		}

		/// <summary>
		/// Saves the page state to persistence medium.
		/// </summary>
		/// <param name="viewState">State of the view.</param>
		protected override void SavePageStateToPersistenceMedium(object viewState)
		{
			if (Convert.ToBoolean(ConfigSettings.ServerSideViewState)) {
				this.SaveViewState(viewState);
				base.SavePageStateToPersistenceMedium("");
			}
			else {
				base.SavePageStateToPersistenceMedium(viewState);
			}
		}

		/// <summary>
		/// Loads the state of the view.
		/// </summary>
		/// <returns>System.Object.</returns>
		protected object LoadViewState()
		{
			object functionReturnValue = null;
			string text1 = "";
			functionReturnValue = null;
			try {
				text1 = base.Request.Form[VIEW_STATE_FIELD_NAME];
				HttpContext.Current.Session[REQUEST_NUMBER] = int.Parse(text1);
				if (((HttpContext.Current.Session[(VIEW_STATE_FIELD_NAME + text1)] != null))) {
					functionReturnValue = Deserialize((byte[])this.Session[(VIEW_STATE_FIELD_NAME + text1)]);
				}
			}
			catch {
			}
			return functionReturnValue;
		}

		/// <summary>
		/// Saves the state of the view.
		/// </summary>
		/// <param name="viewState">State of the view.</param>
		protected void SaveViewState(object viewState)
		{
			int num1 = 0;
			if (((HttpContext.Current.Session[REQUEST_NUMBER] != null))) {
				num1 = ((int)HttpContext.Current.Session[REQUEST_NUMBER] + 1);
				if ((VIEW_STATE_NUM_PAGES == num1)) {
					num1 = 0;
				}
			}
			HttpContext.Current.Session[REQUEST_NUMBER] = num1;
			this.Session[("__vi" + num1.ToString())] = Serialize(viewState);
			this.ClientScript.RegisterHiddenField(VIEW_STATE_FIELD_NAME, num1.ToString());
		}

		/// <summary>
		/// Handles the PreInit event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected void Page_PreInit(object sender, EventArgs e)
		{
			MFunctionProfile mFunction = FunctionUtility.CurrentProfile();
			if (mFunction != null)
			{
				this.EnableViewState = mFunction.EnableViewState;
			}
		}

		private byte[] Serialize(object obj)
		{
			byte[] functionReturnValue = null;
			MemoryStream ms = null;
            LosFormatter formater = null;
            try
            {
                ms = new MemoryStream();
                formater = new LosFormatter();
                formater.Serialize(ms, obj);
                functionReturnValue = ms.ToArray();
            }
            catch (Exception)
            {

                throw new WebSupportException("Could not Serialize the object");
            }
            finally 
            {
                if (ms != null) ms.Close();
                if(formater != null) formater = null;
            }
			return functionReturnValue;
		}

		private object Deserialize(byte[] bytes)
		{
			object functionReturnValue = null;
			MemoryStream ms = new MemoryStream(bytes);
			LosFormatter formater = new LosFormatter();
			functionReturnValue = formater.Deserialize(ms);
			ms.Close();
			ms.Dispose();
			formater = null;
			return functionReturnValue;
		}
    }
}

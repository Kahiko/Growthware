using GrowthWare.Framework.Model.Profiles;
using System;

namespace GrowthWare.WebSupport.BasePages
{
    /// <summary>
    /// used by pages needed access to ClientChoicesState.
    /// Also inerits from GrowthWare.Framework.Web.Base.Page that
    /// stored session on the server keeping the view state returned
    /// to the browser down to a minimum.
    /// </summary>
    public class ClientChoicesPage : BaseWebpage
    {
        /// <summary>
        /// Returns the ClientChoices state from context.
        /// </summary>
        public MClientChoicesState ClientChoicesState
        {
            get { return (MClientChoicesState)Context.Items[MClientChoices.SessionName]; }
        }

        /// <summary>
        /// Handles the PreInit event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);
        }
    }
}

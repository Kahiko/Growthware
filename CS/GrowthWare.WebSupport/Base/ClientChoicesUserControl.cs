using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Base
{
    /// <summary>
    /// Used as the base class for user control objected requiring ClientChoicesState
    /// </summary>
    public class ClientChoicesUserControl : BaseUserControl
    {
        /// <summary>
        /// Gets the state of the client choices.
        /// </summary>
        /// <value>The state of the client choices.</value>
        public MClientChoicesState ClientChoicesState
        {
            get { return (MClientChoicesState)Context.Items[MClientChoices.SessionName]; }
        }
    }
}

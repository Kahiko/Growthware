using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Base
{
    public class ClientChoicesUserControl : BaseUserControl
    {
        /// <summary>
        /// Returns the ClientChoices state from context.
        /// </summary>
        public MClientChoicesState ClientChoicesState
        {
            get { return (MClientChoicesState)Context.Items[MClientChoices.SessionName]; }
        }
    }
}

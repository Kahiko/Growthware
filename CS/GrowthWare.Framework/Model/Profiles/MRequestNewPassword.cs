using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles
{
    public class MRequestNewPassword : MMessageProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MRequestNewPassword"/> class.
        /// </summary>
        public MRequestNewPassword() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRequestNewPassword" /> class.
        /// </summary>
        /// <param name="profile">MMessageProfile</param>
        public MRequestNewPassword(MMessageProfile profile)
            : base(profile)
        {

        }

        /// <summary>
        /// Gets or sets the name of the account.
        /// </summary>
        /// <value>The name of the account.</value>
        public String AccountName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public String FullName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public String Password { get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        /// <value>The server.</value>
        public String Server { get; set; }
    }
}

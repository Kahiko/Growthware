using System;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Model object representing the Request New Password Profile
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MRequestNewPassword : MMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MRequestNewPassword"/> class.
        /// </summary>
        public MRequestNewPassword() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MRequestNewPassword" /> class.
        /// </summary>
        /// <param name="profile">MMessage</param>
        public MRequestNewPassword(MMessage profile)
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

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        public String ResetToken { get; set; }
    }
}

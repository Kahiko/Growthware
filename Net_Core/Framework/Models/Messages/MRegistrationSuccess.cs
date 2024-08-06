using System;

namespace GrowthWare.Framework.Models.Messages;

/// <summary>
/// Model object representing the Request New Password Profile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MRegistrationSuccess : MMessage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MRegistrationSuccess"/> class.
    /// </summary>
    public MRegistrationSuccess() : base() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="MRegistrationSuccess" /> class.
    /// </summary>
    /// <param name="profile">MMessage</param>
    public MRegistrationSuccess(MMessage profile) : base(profile) { }

    public String Email { get; set; }

    /// <summary>
    /// Gets or sets the full name.
    /// </summary>
    /// <value>The full name.</value>
    public String FullName { get; set; }

    /// <summary>
    /// Gets or sets the server.
    /// </summary>
    /// <value>The server.</value>
    public String Server { get; set; }

    /// <summary>
    /// Gets or sets the verification token..
    /// </summary>
    /// <value>The verification token.</value>
    public String VerificationToken { get; set; }

}

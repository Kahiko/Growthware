using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Public interface for DDBInformation
    /// </summary>
    public interface IDBInformation : IDDBInteraction
    {
        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        MDBInformation Profile
        {
            get;
            set;

        }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <returns>DataRow.</returns>
        DataRow GetProfileRow { get; }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool UpdateProfile();
    }
}

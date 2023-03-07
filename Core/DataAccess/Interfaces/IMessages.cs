using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces;
/// <summary>
/// IDMessages defines the contract for any
/// class implementing the interface.
/// </summary>
public interface IMessages : IDBInteraction
{

    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    /// <value>The profile.</value>
    MMessage Profile { get; set; }

    /// <summary>
    /// Gets or sets the security entity seq ID.
    /// </summary>
    /// <value>The security entity seq ID.</value>
    int SecurityEntitySeqId { get; set; }

    /// <summary>
    /// Gets all messages.
    /// </summary>
    /// <returns>DataTable.</returns>
    DataTable Messages();

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <returns>DataRow.</returns>
    DataRow Message(int messageSeqId);

    /// <summary>
    /// Saves this instance.
    /// </summary>
    void Save();
}

using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

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
    /// Gets all messages for the given security entity.
    /// </summary>
    /// <returns>DataTable.</returns>
    /// <remarks>
    /// Calls stored procedure "Get_Messages".
    /// If messages do not exists for the given security entity and the security entity is valid, then new messages are created for the requested security entity.
    /// </remarks>
    Task<DataTable> Messages();

    /// <summary>
    /// Gets the message.
    /// </summary>
    /// <returns>DataRow.</returns>
    DataRow Message(int messageSeqId);

    /// <summary>
    /// Saves this instance.
    /// </summary>
    Task<int> Save();
}

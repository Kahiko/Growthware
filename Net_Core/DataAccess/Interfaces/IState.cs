using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface IState : IDBInteraction
{
    /// <summary>
    /// Used by all methods and must be set to send parameters to the data store
    /// </summary>
    MState Profile { get; set; }

    int SecurityEntitySeqId { get; set; }

    /// <summary>
    /// Returns all states associated with a given SecurityEntitySeqID.
    /// </summary>
    /// <returns>DataTable</returns>
    Task<DataTable> GetStates();

    /// <summary>
    /// Inserts or updates account information
    /// </summary>
    Task Save();

}
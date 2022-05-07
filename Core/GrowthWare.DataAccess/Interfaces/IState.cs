using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface IState: IDBInteraction
    {
        /// <summary>
        /// Used by all methods and must be set to send parameters to the data store
        /// </summary>
        MState Profile {get; set;}

        int SecurityEntitySeqId {get; set;}

        /// <summary>
        /// Deletes an account
        /// </summary>
        void Delete();

        /// <summary>
        /// Retrieves Account information
        /// </summary>
        /// <returns>DataRow</returns>
        DataRow GetState {get;}

        /// <summary>
        /// Returns all states associated with a given SecurityEntitySeqID.
        /// </summary>
        /// <returns>DataTable</returns>
        DataTable GetStates {get;}

        /// <summary>
        /// Inserts or updates account information
        /// </summary>
        void Save();

    }

}

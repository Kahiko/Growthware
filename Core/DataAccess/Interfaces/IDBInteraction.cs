using System;

namespace GrowthWare.DataAccess.Interfaces.Base
{
    /// <summary>
    /// The base interface for Database interaction code
    /// </summary>
    public interface IDBInteraction
    {
        String ConnectionString { get; set; }
    }

}

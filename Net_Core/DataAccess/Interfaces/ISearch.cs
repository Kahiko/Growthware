using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface ISearch : IDBInteraction
{
  /// <summary>
  /// Sets or gets the SecurityEntitySeqID
  /// </summary>
  int SecurityEntitySeqID { get; set; }

  /// <summary>
  /// Gets a subset of information by calling the ZGWSystem.Get_Paginated_Data stored
  /// procedure from the database
  /// </summary>
  /// <param name="searchCriteria"></param>
  /// <returns></returns>
  Task<DataTable> GetSearchResults(MSearchCriteria searchCriteria);
}

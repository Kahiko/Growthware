using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// DClientChoices provides all database interaction to SQL Server 2008
/// </summary>
/// <remarks>
///		The Profile and SecurityEntitySeqID properties must be set
///		before using any methods.
///		Properties where chosen instead of parameters because all
///		methods will need one or both to perform their work.
///	</remarks>
public class DClientChoices : AbstractDBInteraction, IClientChoices
{

#region Constructors
    public DClientChoices(string connectionString): base()
    {
        this.ConnectionString = connectionString;
    }
#endregion

#region Pubic Methods
    DataRow IClientChoices.GetChoices(string account)
    {
        string mStoredProcedure = "ZGWCoreWeb.Get_Account_Choice";
        if (String.IsNullOrEmpty(account)) { throw new ArgumentException("Must set the Account property", nameof(account)); };
        SqlParameter[] myParameters =
        {
            new SqlParameter("@P_ACCOUNT", account)
        };
        return base.GetDataRow(mStoredProcedure, myParameters);
    }

    void IClientChoices.Save(Hashtable clientChoicesStateHashtable)
    {
        if (clientChoicesStateHashtable == null || clientChoicesStateHashtable.Count == 0) { throw new ArgumentNullException(nameof(clientChoicesStateHashtable), "Must set the clientChoicesStateHashTable property"); };
        string mStoredProcedure = "ZGWCoreWeb.Set_Account_Choices";
        IEnumerator HashKeyEnum = ((IEnumerable)clientChoicesStateHashtable.Keys).GetEnumerator();
        IEnumerator HashValEnum = ((IEnumerable)clientChoicesStateHashtable.Values).GetEnumerator();
        SqlParameter[] commandParameters = new SqlParameter[clientChoicesStateHashtable.Count];
        int x = 0;
        while ((HashKeyEnum.MoveNext() & HashValEnum.MoveNext()))
        {
            SqlParameter myParameter = new SqlParameter("@P_" + HashKeyEnum.Current.ToString(), SqlDbType.NVarChar, 1000);
            myParameter.Value = HashValEnum.Current.ToString();
            commandParameters.SetValue(myParameter, x);
            x = x + 1;
        }
        base.ExecuteNonQuery(mStoredProcedure, commandParameters);
    }
#endregion

}

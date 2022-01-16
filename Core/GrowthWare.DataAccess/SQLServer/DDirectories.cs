using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Class DDirectories.
    /// </summary>
    public class DDirectories : DDBInteraction, IDirectories
    {
        DataTable IDirectories.Directories()
        {
            String mStoredProcedure = "ZGWOptional.Get_Directory";
            SqlParameter[] mParameters =
			{
			 new SqlParameter("@P_Function_SeqID", -1)
			};
            return base.GetDataTable(mStoredProcedure, mParameters);
        }

        void IDirectories.Save(MDirectoryProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            String mStoredProcedure = "ZGWOptional.Set_Directory";
            SqlParameter[] mParameters =
			{
			  new SqlParameter("@P_Function_SeqID", profile.FunctionSeqId),
			  new SqlParameter("@P_Directory", profile.Directory),
			  new SqlParameter("@P_Impersonate", profile.Impersonate),
			  new SqlParameter("@P_Impersonating_Account", profile.ImpersonateAccount),
			  new SqlParameter("@P_Impersonating_Password", profile.ImpersonatePassword),
			  new SqlParameter("@P_Added_Updated_By", GetAddedUpdatedBy(profile)),
			  GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output)			
			};
            base.ExecuteNonQuery(mStoredProcedure, mParameters);
        }

        int IDirectories.SecurityEntitySeqId { get; set; }
    }
}

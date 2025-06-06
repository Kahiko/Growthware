﻿using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

public class DDBInformation : AbstractDBInteraction, IDBInformation
{

#region Member Fields
    private MDBInformation m_Profile = null;
#endregion

#region Public Properties
    public MDBInformation Profile { get{return this.m_Profile;} set{this.m_Profile = value;} }
#endregion

#region Constructors
    public DDBInformation(string connectionString): base() 
    {
        this.ConnectionString = connectionString;
    }
#endregion

    public async Task<DataRow> GetProfileRow()
    {
        string mStoredProcedure = "ZGWSystem.Get_Database_Information";
        return await base.GetDataRowAsync(mStoredProcedure);
    }

    public async Task<bool> UpdateProfile()
    {
        string mStoredProcedure = "ZGWSystem.Set_DataBase_Information";
        SqlParameter[] mParameters = [
            new ("@P_Database_InformationSeqId", m_Profile.DatabaseInformationSeqId),
            new ("@P_Version", m_Profile.Version),
            new ("@P_Enable_Inheritance", m_Profile.EnableInheritance),
            new ("@P_Added_Updated_By", GetAddedUpdatedBy(m_Profile, m_Profile.Id)),
            GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.InputOutput)
        ];
        try
        {
                await base.ExecuteNonQueryAsync(mStoredProcedure, mParameters);
                return true;
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}

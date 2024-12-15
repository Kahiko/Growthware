using System;
using System.Data;
using System.Data.SqlClient;
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;

namespace GrowthWare.DataAccess.SQLServer;

public class DFeedbacks : AbstractDBInteraction, IFeedbacks
{
    #region Private Field
        private MFeedback m_Profile = null;
    #endregion

    #region Public Properties
        MFeedback IFeedbacks.Profile
        {
            get { return this.m_Profile; }
            set { this.m_Profile = value; }
        }
    #endregion

    #region Public Methods
        DataRow IFeedbacks.GetFeedback
        {
            get
            {
                String mStoredProcedure = "[ZGWOptional].[Get_Feedback]";
                SqlParameter[] mParameters = {
                    GetSqlParameter("@P_FeedbackId", this.m_Profile.FeedbackId, ParameterDirection.Input)
                };
                return base.GetDataRow(mStoredProcedure, mParameters);
            }
        }

        DataRow IFeedbacks.SaveFeedback
        {
            get
            {
                String mStoredProcedure = "[ZGWOptional].[Set_Feedback]";
                SqlParameter[] mParameters = {
                    new("@P_FeedbackId", this.m_Profile.FeedbackId),
                    new("@P_AssigneeId", this.m_Profile.AssigneeId),
                    new("@P_Date_Closed", this.m_Profile.DateClosed),
                    new("@P_Date_Opened", this.m_Profile.DateOpened),
                    new("@P_Details", this.m_Profile.Details),
                    new("@P_Found_In_Version", this.m_Profile.FoundInVersion),
                    new("@P_FunctionSeqId", this.m_Profile.FunctionSeqId),
                    new("@P_Notes", this.m_Profile.Notes ?? ""),
                    new("@P_Severity", this.m_Profile.Severity ?? ""),
                    new("@P_Status", this.m_Profile.Status),
                    new("@P_SubmittedById ", this.m_Profile.SubmittedById),
                    new("@P_TargetVersion", this.m_Profile.TargetVersion ?? ""),
                    new("@P_Type", this.m_Profile.Type ?? ""),
                    new("@P_UpdatedById", this.m_Profile.UpdatedById),
                    new("@P_VerifiedById", this.m_Profile.VerifiedById),
                    this.GetSqlParameter("@P_Primary_Key", -1, ParameterDirection.Output)
                };
                return base.GetDataRow(mStoredProcedure, mParameters);
            }
        }
    #endregion
}
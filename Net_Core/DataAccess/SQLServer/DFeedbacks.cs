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
        private int m_SecurityEntitySeqID = -2;
    #endregion

    #region Public Properties
        MFeedback IFeedbacks.Profile
        {
            get { return this.m_Profile; }
            set { this.m_Profile = value; }
        }

        int IFeedbacks.SecurityEntitySeqId
        {
            get { return m_SecurityEntitySeqID; }
            set { m_SecurityEntitySeqID = value; }
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
    #endregion
}
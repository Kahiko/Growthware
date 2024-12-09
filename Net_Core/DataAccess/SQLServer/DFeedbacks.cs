using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;

namespace GrowthWare.DataAccess.SQLServer;

public class DFeedbacks : AbstractDBInteraction, IFeedbacks
{
    #region Private Field
        private string m_Profile = null;
        private int m_SecurityEntitySeqID = -2;
    #endregion

    #region Public Properties
        string IFeedbacks.Profile
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
}
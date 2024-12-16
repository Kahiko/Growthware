using System.Data;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class FeedbackUtility
{
    private static BFeedbacks m_BusinessLogic = null;

    private static BFeedbacks getBusinessLogic
    {
        get 
        {
            if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
            {
                m_BusinessLogic = new(SecurityEntityUtility.CurrentProfile);
            }
            return m_BusinessLogic;
        }
    }

    public static UIFeedback GetFeedback(int feedbackId)
    {
        UIFeedback mRetVal = getBusinessLogic.GetFeedback(feedbackId);
        MTestDatabaseTable mTestDatabaseTable = new MTestDatabaseTable();

        mTestDatabaseTable.AssigneeId = 1;
        mTestDatabaseTable.DateOpened = mTestDatabaseTable.DefaultDateTime;
        mTestDatabaseTable.Details = "This is a test description";
        mTestDatabaseTable.FeedbackId = 52;
        mTestDatabaseTable.FunctionSeqId = 1;
        mTestDatabaseTable.FoundInVersion = ConfigSettings.Version;
        mTestDatabaseTable.IsDeleted = false;
        mTestDatabaseTable.Notes = "This is a test note";
        mTestDatabaseTable.Severity = "Test Severity";
        mTestDatabaseTable.Status = "Test Status";
        mTestDatabaseTable.SubmittedById = 1;
        mTestDatabaseTable.TargetVersion = "Test Version";
        mTestDatabaseTable.Type = "Test Type";
        mTestDatabaseTable.UpdatedById = 1;
        mTestDatabaseTable.VerifiedById = 1;

        string mInsertWithParameters = MTestDatabaseTable.GenerateInsertWithParameters<MTestDatabaseTable>();
        string mInsertWithValues = mTestDatabaseTable.GenerateInsertWithValues<MTestDatabaseTable>();
        string mDeleteWithParameters = MTestDatabaseTable.GenerateDeleteWithParameters<MTestDatabaseTable>("FeedbackId");
        string mDeleteWithValues = mTestDatabaseTable.GenerateDeleteWithValues<MTestDatabaseTable>("FeedbackId");
        string mDeleteWithValuesSpecifyKeyValue = mTestDatabaseTable.GenerateDeleteWithValues<MTestDatabaseTable>("FeedbackId", "'yaba'");
        string mUpdateWithParameters = MTestDatabaseTable.GenerateUpdateWithParameters<MTestDatabaseTable>("FeedbackId");
        string mUpdateWithValues = mTestDatabaseTable.GenerateUpdateWithValues<MTestDatabaseTable>("FeedbackId");
        string mUpdateWithValuesSpecifyKeyValue = mTestDatabaseTable.GenerateUpdateWithValues<MTestDatabaseTable>("FeedbackId", "'yaba'");

        DataTable mDataTable = MTestDatabaseTable.GetEmptyTable<MTestDatabaseTable>("Feedbacks", false);
        mDataTable = null;
        mDataTable = MTestDatabaseTable.GetEmptyTable<MTestDatabaseTable>("Feedbacks", true);
        return mRetVal;
    }

    public static UIFeedback SaveFeedback(MFeedback feedback)
    {
        UIFeedback mRetVal = null;
        mRetVal = getBusinessLogic.SaveFeedback(feedback);
        return mRetVal;
    }


}
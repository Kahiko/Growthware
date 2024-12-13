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
        return mRetVal;
    }

    public static UIFeedback SaveFeedback(MFeedback feedback)
    {
        UIFeedback mRetVal = null;
        // mRetVal = getBusinessLogic.SaveFeedback(feedback);
        return mRetVal;
    }


}
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Web.Support.Utilities;

public static class FeedbackUtility
{
/*
 * The feedback feature was added more for the database table desing showing how
 * to keep history of a record in the same table utilizing a "Identifier" column,
 * StartDate and EndDate.  A null EndDate means the record is current.
 */
    private static BFeedbacks m_BusinessLogic = null;
    private static readonly Logger m_Logger = Logger.Instance();

    /// <summary>
    /// Retrieves the business logic instance.
    /// </summary>
    private static BFeedbacks BusinessLogic
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

    /// <summary>
    /// Retrieves a feedback item from the database.
    /// </summary>
    /// <param name="feedbackId">The ID of the feedback to retrieve.</param>
    /// <returns>The retrieved feedback item.</returns>
    /// <exception cref="ArgumentNullException">The supplied ID is null.</exception>
    /// <exception cref="NullReferenceException">The feedback with the ID was not found.</exception>
    /// <exception cref="Exception">There was an error retrieving the feedback.</exception>
    public static async Task<UIFeedback> GetFeedback(int feedbackId)
    {
        try
        {
            UIFeedback mRetVal = await BusinessLogic.GetFeedback(feedbackId);
            if (mRetVal == null)
            {
                throw new NullReferenceException($"The feedback with the ID {feedbackId} was not found.");
            }
            return mRetVal;
        }
        catch (Exception ex)
        {
            string mMessage = $"There was an error getting feedback with the ID {feedbackId}. {ex.Message}";
            m_Logger.Error(mMessage);
            throw;
        }
    }

    /// <summary>
    /// Saves a feedback item to the database. If the feedback with the supplied ID already exists, it will be updated.
    /// </summary>
    /// <param name="feedback">The feedback to be saved.</param>
    /// <returns>The feedback that was saved.</returns>
    /// <exception cref="ArgumentNullException">The supplied feedback is null.</exception>
    /// <exception cref="NullReferenceException">The saved feedback was null.</exception>
    /// <exception cref="Exception">There was an error saving the feedback.</exception>
    public static async Task<UIFeedback> SaveFeedback(MFeedback feedback)
    {
        if (feedback == null)
        {
            throw new ArgumentNullException(nameof(feedback), "feedback cannot be null!");
        }

        try
        {
            UIFeedback mRetVal = await BusinessLogic.SaveFeedback(feedback);
            if (mRetVal == null)
            {
                throw new NullReferenceException("The saved feedback was null.");
            }
            return mRetVal;
        }
        catch (Exception ex)
        {
            string mMessage = $"There was an error saving the feedback. {ex.Message}";
            m_Logger.Error(mMessage);
            throw;
        }
    }
}

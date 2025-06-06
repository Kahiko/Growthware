using System;
using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Framework;

namespace GrowthWare.Web.Support.Helpers;
/// <summary>
/// SessionHelper class contains methods for managing session data.
/// </summary>
/// <notes>
/// TODO: this works fine for a single server but not for a clustered environment.
///     1.) Could use a distributed cache for session data such as SQL Server.
/// May want to consider using SQL Dependency with SignalR to manage the frontend updates that relay on the session data.
///     such as menu data or security data.  Perhaps after I've finished the bulk if not all of the current UI.
/// </notes>
public static class SessionHelper
{
    private static IHttpContextAccessor m_HttpContextAccessor;
    private static Logger m_Logger = Logger.Instance(); 

    /// <summary>
    /// Adds a value to the session with the specified session name.
    /// </summary>
    /// <param name="sessionName">The name of the session.</param>
    /// <param name="value">The value to be added to the session.</param>
    public static void AddToSession(string sessionName, object value)
    {
        if (value != null)
        {
            try
            {
                string mJsonString = JsonSerializer.Serialize(value);
                m_HttpContextAccessor.HttpContext.Session.SetString(sessionName, mJsonString);
            }
            catch (System.Exception ex)
            {
                MLoggingProfile mLoggingProfile = new MLoggingProfile();
                mLoggingProfile.Account = "System";
                mLoggingProfile.ClassName = "SessionHelper";
                mLoggingProfile.Component = "Web.Support";
                mLoggingProfile.Level = "Error";
                mLoggingProfile.LogDate = DateTime.Now;
                mLoggingProfile.MethodName = "AddToSession";
                mLoggingProfile.Msg = ex.Message;
                m_Logger.Error(mLoggingProfile);
                throw;
            }
        }
    }

    private static bool deserializeFilter(Exception e)
    {
        return e is ArgumentNullException ||
                e is JsonException ||
                e is NotSupportedException ||
                e is InvalidOperationException;
    }

    /// <summary>
    /// Retrieves an object of type T from the session using the provided session name.
    /// </summary>
    /// <typeparam name="T">The type of the object to be retrieved from the session.</typeparam>
    /// <param name="sessionName">The name of the session key.</param>
    /// <returns>
    /// The object of type T retrieved from the session. If the object cannot be retrieved,
    /// default(T) is returned.
    /// </returns>
    public static T GetFromSession<T>(string sessionName)
    {
        string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(sessionName);
        if (mJsonString == null) return default(T);
        try
        {
            return JsonSerializer.Deserialize<T>(mJsonString);
        }
        catch (Exception ex) when (deserializeFilter(ex))
        {
            MLoggingProfile mLoggingProfile = new MLoggingProfile();
            mLoggingProfile.Account = "System";
            mLoggingProfile.ClassName = "SessionHelper";
            mLoggingProfile.Component = "Web.Support";
            mLoggingProfile.Level = "Error";
            mLoggingProfile.LogDate = DateTime.Now;
            mLoggingProfile.MethodName = "GetFromSession";
            mLoggingProfile.Msg = "sessionName: " + sessionName + " ex: " + ex.Message;
            m_Logger.Error(mLoggingProfile);
            return default(T);
        }
    }

    /// <summary>
    /// Removes all items from the session.
    /// </summary>
    public static void RemoveAll()
    {
        m_HttpContextAccessor.HttpContext.Session.Clear();
    }

    /// <summary>
    /// Removes a value from the session with the specified session name.
    /// </summary>
    /// <param name="sessionName">The name of the session from which to remove the value.</param>
    /// <param name="all">
    /// A flag indicating whether to remove all values from the session. 
    /// The default value is false.
    /// </param>
    public static void RemoveFromSession(string sessionName, bool all = false)
    {
        if (!all)
        {
            m_HttpContextAccessor.HttpContext.Session.Remove(sessionName);
        }
        else
        {
            m_HttpContextAccessor.HttpContext.Session.Clear();
        }
    }

    /// <summary>
    /// Set the HttpContextAccessor used by the class.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }
}
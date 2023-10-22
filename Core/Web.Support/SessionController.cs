using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public static class SessionController
{
    private static IHttpContextAccessor m_HttpContextAccessor;

    public static void AddToSession(string sessionName, object value)
    {
        string mJsonString = JsonSerializer.Serialize(value);
        m_HttpContextAccessor.HttpContext.Session.SetString(sessionName, mJsonString);
    }

    public static T GetFromSession<T>(string sessionName)
    {
        string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(sessionName);
        try
        {
            return JsonSerializer.Deserialize<T>(mJsonString);
        }
        catch (System.Exception)
        {
            return default(T);
        }
    }

    public static void RemoveAll()
    {
        m_HttpContextAccessor.HttpContext.Session.Clear();
    }

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

    [CLSCompliant(false)]
    public static void SetHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
    }
}

using System.Collections.Generic;
using System.Security;

namespace GrowthWare.Framework;

public static class DBLogColumns
{

    public const string Account = "[Account]";
    public const string Component = "[Component]";
    public const string ClassName = "[ClassName]";
    public const string Level = "[Level]";
    public const string LogDate = "[LogDate]";
    public const string LogSeqId = "[LogSeqId]";
    public const string MethodName = "[MethodName]";
    public const string Msg = "[Msg]";

    public static string GetCommaSeparatedString() => $"{Account}, {Component}, {ClassName}, {Level}, {LogDate}, {LogSeqId}, {MethodName}, {Msg}";
    
    public static List<string> GetList() => [Account, Component, ClassName, Level, LogDate, LogSeqId, MethodName, Msg];
}
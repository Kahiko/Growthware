namespace GrowthWare.Framework.Enumerations;

public enum LogDestination
{
    /// <summary>
    /// Log to File
    /// </summary>
    File = 0,

    /// <summary>
    /// Log to Console (Here for UI)
    /// </summary>
    Console = 1,

    /// <summary>
    /// Log to Database
    /// </summary>
    DB = 2,

    /// <summary>
    /// Log to Toast (Here for UI)
    /// </summary>
    Toast = 3,
}
using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Models;
/// <summary>
/// Class DTO_SecurityEntity_RegistrationInfo is a compound class of the
/// MSecurityEntity and MRegistrationInformation objects that is 
/// used to transfer data between the UI and the API
/// </summary>
/// <seealso cref="GrowthWare.Framework.Models.DTO_SecurityEntity_RegistrationInfo" />
public class DTO_SecurityEntity_RegistrationInfo
{
    public MSecurityEntity SecurityEntity { get; set; }
    public MRegistrationInformation RegistrationInformation { get; set; }
}
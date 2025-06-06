﻿using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

/// <summary>
/// Public interface for DDBInformation
/// </summary>
public interface IDBInformation : IDBInteraction
{
    /// <summary>
    /// Gets or sets the profile.
    /// </summary>
    /// <value>The profile.</value>
    MDBInformation Profile { get; set; }

    /// <summary>
    /// Gets the profile.
    /// </summary>
    /// <returns>DataRow.</returns>
    Task<DataRow> GetProfileRow();

    /// <summary>
    /// Updates the profile.
    /// </summary>
    /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
    Task<bool> UpdateProfile();
}
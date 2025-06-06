﻿using System;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Interfaces;

/// <summary>
/// Interface for anything needed group and role security
/// </summary>
[CLSCompliant(true)]
public interface IGroupRoleSecurity
{
    /// <summary>
    /// Roles that are directly assigned
    /// </summary>
    Collection<String> AssignedRoles
    {
        get;
    }

    /// <summary>
    /// Roles that are derived from groups
    /// </summary>
    Collection<String> DerivedRoles
    {
        get;
    }

    /// <summary>
    /// Groups that are directly assigned
    /// </summary>
    Collection<String> Groups
    {
        get;
    }
}

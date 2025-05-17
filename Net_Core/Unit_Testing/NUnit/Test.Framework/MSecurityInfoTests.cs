using GrowthWare.Framework.Models;
using GrowthWare.Framework.Interfaces;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Tests;

[TestFixture]
public class MSecurityInfoTests : MSecurityInfo
{
    [Test]
    public void Constructor_DefaultValues_False()
    {
        // Arrange
        MSecurityInfo securityInfo = new();

        // Assert
        Assert.That(securityInfo.MayView, Is.False);
        Assert.That(securityInfo.MayAdd, Is.False);
        Assert.That(securityInfo.MayEdit, Is.False);
        Assert.That(securityInfo.MayDelete, Is.False);
    }

    [Test]
    public void SetMemberFields_ViewPermission_True()
    {
        // Arrange
        IGroupRolePermissionSecurity groupRolePermissionSecurity = new MockGroupRolePermissionSecurity();
        IGroupRoleSecurity groupRoleSecurity = new MockGroupRoleSecurity();

        // Act
        MSecurityInfo securityInfo = new(groupRolePermissionSecurity, groupRoleSecurity);

        // Assert
        Assert.That(securityInfo.MayView, Is.True);
    }

    [Test]
    public void SetMemberFields_AddPermission_True()
    {
        // Arrange
        IGroupRolePermissionSecurity groupRolePermissionSecurity = new MockGroupRolePermissionSecurity();
        IGroupRoleSecurity groupRoleSecurity = new MockGroupRoleSecurity(new Collection<string>(), new Collection<string> { "Role2" });

        // Act
        MSecurityInfo securityInfo = new(groupRolePermissionSecurity, groupRoleSecurity);
        // Assert
        Assert.That(securityInfo.MayAdd, Is.True);
    }

    [Test]
    public void SetMemberFields_EditPermission_True()
    {
        // Arrange
        IGroupRolePermissionSecurity groupRolePermissionSecurity = new MockGroupRolePermissionSecurity();
        IGroupRoleSecurity groupRoleSecurity = new MockGroupRoleSecurity(new Collection<string>(), new Collection<string> { "Role3" });

        // Act
        MSecurityInfo securityInfo = new(groupRolePermissionSecurity, groupRoleSecurity);

        // Assert
        Assert.That(securityInfo.MayEdit, Is.True);
    }

    [Test]
    public void SetMemberFields_DeletePermission_True()
    {
        // Arrange
        IGroupRolePermissionSecurity groupRolePermissionSecurity = new MockGroupRolePermissionSecurity();
        IGroupRoleSecurity groupRoleSecurity = new MockGroupRoleSecurity(new Collection<string>(), new Collection<string> { "Role4" });

        // Act
        MSecurityInfo securityInfo = new(groupRolePermissionSecurity, groupRoleSecurity);

        // Assert
        Assert.That(securityInfo.MayDelete, Is.True);
    }

    [Test]
    public void CheckGroups_NoGroups_False()
    {
        // Arrange
        Collection<string> permissionGroups = new Collection<string>();
        Collection<string> profileGroups = new Collection<string>();

        // Act
        bool result = MSecurityInfo.CheckGroups(permissionGroups, profileGroups);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void CheckGroups_ProfileGroupMatchesPermissionGroup_True()
    {
        // Arrange
        Collection<string> permissionGroups = new Collection<string> { "Group1" };
        Collection<string> profileGroups = new Collection<string> { "Group1" };

        // Act
        bool result = MSecurityInfo.CheckGroups(permissionGroups, profileGroups);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public void CheckRoles_NoRoles_False()
    {
        // Arrange
        Collection<string> permissionRoles = new Collection<string>();
        Collection<string> profileRoles = new Collection<string>();

        // Act
        bool result = MSecurityInfo.CheckRoles(permissionRoles, profileRoles);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public void CheckRoles_ProfileRoleMatchesPermissionRole_True()
    {
        // Arrange
        Collection<string> permissionRoles = new Collection<string> { "Role1" };
        Collection<string> profileRoles = new Collection<string> { "Role1" };

        // Act
        bool result = MSecurityInfo.CheckRoles(permissionRoles, profileRoles);

        // Assert
        Assert.That(result, Is.True);
    }
}

public class MockGroupRolePermissionSecurity : IGroupRolePermissionSecurity
{
    public Collection<string> ViewGroups { get; set; } = new Collection<string> { "Group1" };
    public Collection<string> AddGroups { get; set; } = new Collection<string> { "Group2" };
    public Collection<string> EditGroups { get; set; } = new Collection<string> { "Group3" };
    public Collection<string> DeleteGroups { get; set; } = new Collection<string> { "Group4" };
    public Collection<string> DerivedViewRoles { get; set; } = new Collection<string> { "Role1" };
    public Collection<string> DerivedAddRoles { get; set; } = new Collection<string> { "Role2" };
    public Collection<string> DerivedEditRoles { get; set; } = new Collection<string> { "Role3" };
    public Collection<string> DerivedDeleteRoles { get; set; } = new Collection<string> { "Role4" };

    public Collection<string> AssignedViewRoles { get; set; } = new Collection<string>();
    public Collection<string> AssignedAddRoles { get; set; } = new Collection<string>();
    public Collection<string> AssignedEditRoles { get; set; } = new Collection<string>();
    public Collection<string> AssignedDeleteRoles { get; set; } = new Collection<string>();
}

public class MockGroupRoleSecurity : IGroupRoleSecurity
{

    public MockGroupRoleSecurity()
    { 

    }

    public MockGroupRoleSecurity(Collection<string> groups, Collection<string> derivedRoles) 
    {
        if (groups != null) Groups = groups;
        if (derivedRoles != null) DerivedRoles = derivedRoles;
    }
    public Collection<string> Groups { get; set; } = new Collection<string> { "Group1" };
    public Collection<string> DerivedRoles { get; set; } = new Collection<string> { "Role1" };
    public Collection<string> AssignedRoles { get; set; } = new Collection<string> { "Role1" };
}
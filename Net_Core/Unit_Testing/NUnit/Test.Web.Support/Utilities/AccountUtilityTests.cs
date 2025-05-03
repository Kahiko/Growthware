using System;
using Microsoft.AspNetCore.Http;
using Moq;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;
using GrowthWare.Web.Support.Helpers;
using GrowthWare.Web.Support.Utilities;
using GrowthWare.Test.Web.Support;
using System.Reflection;
using GrowthWare.Framework;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Test.Web.Support.Utilities;

[TestFixture]
public class AccountUtilityTests
{

    private string m_Developer_Account = "developer";
    private string m_Mike_Account = "mike";
    private Mock<IHttpContextAccessor> m_HttpContextAccessorMock;
    private Mock<HttpContext> m_HttpContextMock;
    private string m_IpAddress = "127.0.0.1";
    // private string m_Origin = "https://127.0.0.1";
    private string m_Password = "none";
    private MockSession m_SessionMock;
    private MAccountProfile m_Developer_Profile = new();
    private MAccountProfile m_Mike_Profile = new();
    private string m_Developer_Token = "Needs to be set in setup for each run";
    private string m_Mike_Token = "Needs to be set in setup for each run";

    [SetUp]
    public async Task Setup()
    {
        m_HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        m_HttpContextMock = new Mock<HttpContext>();
        m_SessionMock = new MockSession();

        m_HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(m_HttpContextMock.Object);
        m_HttpContextMock.Setup(x => x.Session).Returns(m_SessionMock);

        // Set the mocked IHttpContextAccessor in the SessionHelper so that AccountUtility can use it
        SessionHelper.SetHttpContextAccessor(m_HttpContextAccessorMock.Object);

        /*
         * Ok I know that unit testing is not meant to use any other part of the system.
         * Using any part of the system starts to become an integration test and muddys the water.
         * So using the AccountUtility class to test the AccountUtility class is not optimal.
         *
         * That being said I am the only developer and need to choose where I spend time.
         * In the end I want to be able to unit test the AccountUtility class as well as
         * learn how to make of NUnit tests.
         *
         * I would love to get data from the database directly and hand get all the information
         * I need to test the AccountUtility class in a pure unit test.
         */

        MethodInfo[] mInfos = GWCommon.GetMethods(typeof(AccountUtility));
        List<string> mMethodNames = new List<string>();
        foreach (MethodInfo mInfo in mInfos)
        {
            Console.WriteLine(mInfo.Name);
            mMethodNames.Add(mInfo.Name);
        }

        m_Developer_Profile = await AccountUtility.GetAccount(m_Developer_Account, true);
        m_Mike_Profile = await AccountUtility.GetAccount(m_Mike_Account, true);

        if (m_Developer_Profile == null || m_Mike_Profile == null)
        {
            throw new Exception("Could not find developer or mike account");
        }
        if (m_Developer_Profile != null && m_Developer_Profile.RefreshTokens != null && m_Developer_Profile.RefreshTokens.Count > 0)
        {
            m_Developer_Token = m_Developer_Profile.RefreshTokens
                    .OrderByDescending(obj => obj.Created)
                    .First().Token;
        }
        if (m_Mike_Profile != null && m_Mike_Profile.RefreshTokens != null && m_Mike_Profile.RefreshTokens.Count > 0)
        {
            m_Mike_Token = m_Mike_Profile.RefreshTokens
                    .OrderByDescending(obj => obj.Created)
                    .First().Token;
        }
    }
    [TearDown]

    public void TearDown()
    {
        // Dispose of any resources used in the tests
        m_Developer_Profile?.Dispose();
        m_Mike_Profile?.Dispose();
    }

    [Test]
    public async Task Authenticate_ValidCredentials_ReturnsAccountProfile()
    {
        // Act
        var result = await AccountUtility.Authenticate(m_Developer_Account, m_Password, m_IpAddress);

        // Assert
        Assert.That(result, Is.Not.Null, "The result should not be null.");
        Assert.That(result.Account, Is.EqualTo(m_Developer_Account).Using(StringComparer.OrdinalIgnoreCase), "The result.Account and account should match.");
    }

    [Test]
    public async Task ChangePassword_ValidChange_ReturnsSuccessMessage()
    {
        // Arrange
        var changePassword = new UIChangePassword
        {
            OldPassword = "none",
            NewPassword = "none"
        };
        Tuple<string, MAccountProfile> result = new("Did not attempt to change password", new MAccountProfile());
        // AccountUtility.ChangePassword uses the CurrentProfile so in order to update the CurrentProfile
        // to the profile for m_Mike we need to Authenticate using the "Mike" account.
        MAccountProfile mAccountProfile = await AccountUtility.Authenticate(m_Mike_Account, m_Password, m_IpAddress);
        // Get the CurrentProfile
        MAccountProfile mCurrentProfile = await AccountUtility.CurrentProfile();

        // Act - Change the password if the account is "Mike"
        // Since the "CurrentPofile" is used when calling AccountUtility.ChangePassword as the account whose
        // password is being changed we do not want to change the password if the CurrentProfile is not 
        // the profile for "Mike" (our test account for this test).
        if (mCurrentProfile.Account.ToUpper() != m_Mike_Account.ToUpper())
        {
            Assert.Fail($"The CurrentProfile is not the profile for '{m_Mike_Account}'");
        } 
        else 
        {
            // Change the password and in doing so the get a new Profile in "Item2" with an updated .PasswordLastSet property
            result = await AccountUtility.ChangePassword(changePassword, m_IpAddress);
        }
        
        // Assert
        Assert.Multiple(() =>
        {
            // Verify that the CurrentProfile is the profile for m_Mike
            Assert.That(mCurrentProfile.Account.ToUpper(), Is.EqualTo(m_Mike_Account.ToUpper()), $"The CurrentProfile.Account '{mCurrentProfile.Account}' is not equal to '{m_Mike_Account}'.");
            // Verify that the password has been changed by comparing the password last set between the CurrentProfile and the result of the AccountUtility.ChangePassword
            Assert.That(result.Item2.PasswordLastSet, Is.Not.EqualTo(mAccountProfile.PasswordLastSet), $"The password last '{result.Item2.PasswordLastSet}'");
            // Verify that the message returned is "Your password has been changed."
            Assert.That(result.Item1, Is.EqualTo("Your password has been changed."), "The retruned message should be 'Your password has been changed.' but was: " + result.Item1);
        });
    }

    [Test]
    public void Delete_ValidAccountSeqId_DeletesAccount()
    {
        // Arrange
        // int accountSeqId = 1; // Example account sequence ID

        // Act
        // AccountUtility.Delete(accountSeqId);

        // Assert
        Assert.Fail("Need to implement");
        // Verify that the account has been deleted, possibly by checking the state of the database or using a mock
    }

    [Test]
    public void ForgotPassword_ValidAccount_SendsResetToken()
    {
        // Act
        // var result = AccountUtility.ForgotPassword(m_Account, m_Origin);

        // Assert
        Assert.Fail("Need to implement");
        // Assert.That(result, Is.Not.Null);
        // Additional assertions based on expected behavior
    }

    [Test]
    public async Task GetAccount_ValidAccount_ReturnsAccountProfile()
    {
        // Act
        var result = await AccountUtility.GetAccount(m_Developer_Account);

        // Assert
        Assert.That(result, Is.Not.Null, "The account profile should not be null.");
        Assert.That(result.Account, Is.Not.Null, "The profile.Account should not be null.");
        Assert.That(result.Account.ToUpper(), Is.EqualTo(m_Developer_Account.ToUpper()), "The result.Account and account should match.");
    }

    [Test]
    public async Task GetAccount_InValidAccount_ReturnsAccountProfile()
    {
        // Act
        var result = await AccountUtility.GetAccount("InvalidAccount");

        // Assert
        Assert.That(result.Account, Is.Null, "The Profile.Account should be null.");
    }

    [Test]
    public void GetMenuItems_ValidAccount_ReturnsMenuItems()
    {
        // Arrange
        MenuType mMenuType = MenuType.Horizontal; // Example menu type

        // Act
        var mResult = AccountUtility.GetMenuItems(m_Developer_Account, mMenuType);

        // Assert
        Assert.That(mResult, Is.Not.Null);
    }

    [Test]
    public void GetMenuData_ValidAccountAndMenuType_ReturnsMenuData()
    {
        // Arrange
        MenuType mMenuType = MenuType.Horizontal; // Example menu type

        // Act
        var mResult = AccountUtility.GetMenuData(m_Developer_Account, mMenuType);

        // Assert
        Assert.That(mResult, Is.Not.Null, "Menu data should not be null.");
        // Additional assertions can be added here to verify the content of the menu data
    }

    [Test]
    public void GetProfileByResetToken_ValidToken_ReturnsAccountProfile()
    {
        // Arrange
        // string resetToken = "validResetToken";

        // Act
        // var result = AccountUtility.GetProfileByResetToken(resetToken);

        // Assert
        Assert.Fail("Need to implement");
        // Assert.That(result, Is.Not.Null);
    }

    [Test]
    public async Task Logoff_ValidAccount_LogsOut()
    {
        // Act
        await AccountUtility.Logoff(m_Developer_Account, m_Developer_Token, m_IpAddress);

        // Assert
        // TODO: Need to add assertions -- perhaps could .CurrentProfile and verify that
        // it is the Anonymous account

        // You may need to verify that the account is logged off, depending on your implementation
    }

    [Test]
    public void RefreshToken_ValidToken_ReturnsNewToken()
    {
        // Arrange

        // Act
        // var result = AccountUtility.RefreshToken(m_Token, m_IpAddress);

        // Assert
        Assert.Fail("Need to implement");
        // Assert.That(result, Is.Not.Null);
        // Additional assertions based on expected behavior
    }

    [Test]
    public void Register_ValidAccount_ReturnsSuccess()
    {
        // Arrange
        // MAccountProfile newAccount = new MAccountProfile { Account = "newAccount" };

        // Act
        // var result = AccountUtility.Register(newAccount, m_Origin);

        // Assert
        Assert.Fail("Need to implement");
        // Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void RemoveInMemoryInformation_ValidAccount_RemovesInformation()
    {
        // Act
        AccountUtility.RemoveInMemoryInformation(m_Mike_Account);

        // Assert
        Assert.Fail("Need to implement");
        // Verify that the in-memory information for the account has been cleared
    }

    [Test]
    public void ResetPassword_ValidToken_ResetsPassword()
    {
        // Arrange
        // MAccountProfile mAccountProfile = AccountUtility.GetAccount(m_Account);
        // string newPassword = "newPassword"; // New password

        // Act
        // AccountUtility.ResetPassword(mAccountProfile, newPassword);

        // Assert
        Assert.Fail("Need to implement");
        // Additional assertions to verify that the password has been reset
    }

    [Test]
    public void RevokeToken_ValidToken_RevokesToken()
    {
        // Act
        // AccountUtility.RevokeToken(m_Token, m_IpAddress);

        // Assert
        Assert.Fail("Need to implement");
        // Verify that the token has been revoked, possibly by checking the state of the database or using a mock
    }

    [Test]
    public async Task Save_ValidAccountProfile_SavesProfile()
    {
        // Arrange
        MAccountProfile mProfileToSave = await AccountUtility.GetAccount(m_Mike_Account);

        // Act
        // AccountUtility.Save(mProfileToSave, true, true, true);

        // Assert
        Assert.Fail("Need to implement");
        // Verify that the account profile has been saved, possibly by checking the state of the database or using a mock
    }

    [Test]
    public void VerifyAccount_ValidToken_ReturnsAccountProfile()
    {
        // Arrange
        // // TODO: Replace with a valid verification token
        // string verificationToken = "validToken";

        // Act
        // var result = AccountUtility.VerifyAccount(verificationToken, m_Email);

        // Assert
        Assert.Fail("Need to implement");
        // Assert.That(result, Is.Not.Null);
        // Additional assertions based on expected behavior
    }
}

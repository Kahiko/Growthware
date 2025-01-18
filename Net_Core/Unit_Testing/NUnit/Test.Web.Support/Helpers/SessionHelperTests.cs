using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Session;
using System.Text.Json;
using GrowthWare.Web.Support.Helpers;
using GrowthWare.Test.Web.Support;

namespace GrowthWare.Test.Web.Support.Helpers.Tests;

[TestFixture]
public class SessionHelperTests
{
    private Mock<IHttpContextAccessor> m_HttpContextAccessorMock;
    private Mock<HttpContext> m_HttpContextMock;
    private MockSession m_SessionMock;

    [SetUp]
    public void Setup()
    {
        m_HttpContextAccessorMock = new Mock<IHttpContextAccessor>();
        m_HttpContextMock = new Mock<HttpContext>();
        m_SessionMock = new MockSession();

        m_HttpContextAccessorMock.Setup(x => x.HttpContext).Returns(m_HttpContextMock.Object);
        m_HttpContextMock.Setup(x => x.Session).Returns(m_SessionMock);

        // Set the mocked IHttpContextAccessor in the SessionHelper
        SessionHelper.SetHttpContextAccessor(m_HttpContextAccessorMock.Object);
    }

    [Test]
    public void AddToSession_ShouldStoreValueInSession()
    {
        // Arrange
        var mKeyName = "TestKey";
        var mValue = "TestValue";

        // Act
        SessionHelper.AddToSession(mKeyName, mValue);

        // Assert
        var mResult = SessionHelper.GetFromSession<string>(mKeyName);
        Assert.That(mResult, Is.EqualTo(mValue));
    }

    [Test]
    public void GetFromSession_ShouldRetrieveValueFromSession()
    {
        // Arrange
        var mKeyName = "TestKey";
        var mExpectedValue = "TestValue";
        SessionHelper.AddToSession(mKeyName, mExpectedValue);

        // Act
        var mResult = SessionHelper.GetFromSession<string>(mKeyName);

        // Assert
        Assert.That(mResult, Is.EqualTo(mExpectedValue));
    }

    [Test]
    public void RemoveFromSession_ShouldRemoveValueFromSession()
    {
        // Arrange
        var mKeyName = "TestKey";
        var mValue = "TestValue";
        SessionHelper.AddToSession(mKeyName, mValue);
        // Act
        SessionHelper.RemoveFromSession(mKeyName);

        // Assert
        var mResult = SessionHelper.GetFromSession<string>(mKeyName);
        Assert.That(mResult, Is.Null);
    }

    [Test]
    public void RemoveAll_ShouldClearAllSessionValues()
    {
        // Arrange
        SessionHelper.AddToSession("Key1", "Value1");
        SessionHelper.AddToSession("Key2", "Value2");

        // Act
        SessionHelper.RemoveAll();

        // Assert
        var mResult = SessionHelper.GetFromSession<string>("Key2");
        Assert.That(mResult, Is.Null);
    }

}

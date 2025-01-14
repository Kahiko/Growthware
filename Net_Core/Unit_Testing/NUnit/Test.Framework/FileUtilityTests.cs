using NUnit.Framework;
using System.IO;
using System.Text;
using GrowthWare.Framework;

namespace GrowthWare.Framework.Tests;

[TestFixture]
public class FileUtilityTests
{
    [Test]
    public void GetLineCount_ValidDirectory_ReturnsLineCount()
    {
        // Arrange
        // var directory = new DirectoryInfo("path_to_test_directory");
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var outputBuilder = new StringBuilder();
        var excludeList = new List<string>();
        int directoryLineCount = 0;
        int totalLinesOfCode = 0;
        string[] fileArray = new string[] { "*.cs" };

        // Act
        var result = FileUtility.GetLineCount(directory, 0, outputBuilder, excludeList, directoryLineCount, ref totalLinesOfCode, fileArray);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void GetParent_ValidPath_ReturnsParentDirectory()
    {
        // Arrange
        // string path = "path_to_a_directory";
        string path = Directory.GetCurrentDirectory();

        // Act
        var result = FileUtility.GetParent(path);

        // Assert
        Assert.That(result, Is.Not.Null);
    }

    [Test]
    public void CountDirectory_ValidDirectory_ReturnsCorrectCount()
    {
        // Arrange
        // var directory = new DirectoryInfo("path_to_test_directory");
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        var outputBuilder = new StringBuilder();
        var excludeList = new List<string>();
        string[] fileArray = new string[] { "*.cs" };
        int directoryLineCount = 0;

        // Act
        FileUtility.CountDirectory(directory, outputBuilder, excludeList, fileArray, ref directoryLineCount);

        // Assert
        Assert.That(directoryLineCount, Is.EqualTo(0));
    }

    [Test]
    public void CreateDirectory_ValidPath_CreatesDirectory()
    {
        // Arrange
        string newDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp");

        // Act
        var result = FileUtility.CreateDirectory(newDirectory);

        // Assert
        Assert.That(result, Is.EqualTo("Successfully created the new directory!"));
        
        // Cleanup
        DirectoryInfo mDirectoryInfo = new DirectoryInfo(newDirectory);
        mDirectoryInfo.Delete();
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace C3624738.Tests
{
    [TestClass]
    public class CommandParserSyntaxTests
    {
        private Mock<IGraphical> mockGraphicsGen;
        private Mock<PictureBox> mockPictureBox;
        private CommandParser commandParser;

        [TestInitialize]
        public void SetUp()
        {
            mockGraphicsGen = new Mock<IGraphical>();
            mockPictureBox = new Mock<PictureBox>();
            commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);
        }

        [TestMethod]
        public void CheckSyntax_IncorrectCommand_ShouldDetectSyntaxError()
        {
            string incorrectSyntax = "circl 10";  // Intentionally incorrect command
            var errors = commandParser.CheckSyntax(incorrectSyntax);
            Assert.IsTrue(errors.Count == 1, "Expected syntax errors were not detected.");
            Assert.IsTrue(errors.Any(error => error.Contains("circl")), "Expected specific syntax error message not found.");
        }

        [TestMethod]
        public void CheckSyntax_CorrectCommand_ShouldNotReportError()
        {
            string correctSyntax = "circle 10";  // Correct command
            var errors = commandParser.CheckSyntax(correctSyntax);
            Assert.IsTrue(errors.Count == 0, "Unexpected syntax errors were detected.");
        }

        [TestMethod]
        public void CheckSyntax_MultipleCommands_ShouldDetectErrorsCorrectly()
        {
            // Arrange
            string multipleCommands = "ciclse 20\n" +
                                      "circle 20\n" +
                                      "recngle\n" +
                                      "rectangle 50 50";

            // Act
            var errors = commandParser.CheckSyntax(multipleCommands);

            // Assert
            Assert.AreEqual(2, errors.Count, "Incorrect number of syntax errors detected.");
            Assert.IsTrue(errors.Any(error => error.Contains("ciclse")), "Expected syntax error message for 'ciclse' not found.");
            Assert.IsTrue(errors.Any(error => error.Contains("recngle")), "Expected syntax error message for 'recngle' not found.");
        }
    }
}

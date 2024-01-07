using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738.Tests
{
    /// <summary>
    /// Tests for the CommandParser Variables.
    /// </summary>
    [TestClass]
    public class CommandParserVariableTests
    {
        private Mock<IGraphical> mockGraphicsGen;
        private Mock<PictureBox> mockPictureBox;
        private CommandParser commandParser;

        /// <summary>
        /// Sets up the test environment.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            mockGraphicsGen = new Mock<IGraphical>();
            mockPictureBox = new Mock<PictureBox>();
            commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);
        }

        /// <summary>
        /// Tests the ParseCommand method with declaring and using a variable with a circle command.
        /// </summary>
        [TestMethod]
        public void ParseCommand_DeclareAndUseVariableWithCircle_CommandExecutedCorrectly()
        {
            // Arrange
            string declareVariable = "radius = 10";
            string circleWithVariable = "circle radius";

            // Act
            commandParser.ParseCommand(declareVariable);
            commandParser.ParseHandler("run", circleWithVariable);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 10), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method with expressions used to draw concentric circles.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExpressionsUsedToDrawConcentricCircles_CommandsExecutedCorrectly()
        {
            // Arrange
            string declareCount = "count = 5"; 
            string sizeExpression = "size = count * 10";
            string drawConcentricCircles = "loop count\ncircle size\nsize = size - 10\nendloop";

            // Act
            commandParser.ParseCommand(declareCount);
            commandParser.ParseCommand(sizeExpression);
            commandParser.ParseHandler("run", drawConcentricCircles);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 50), Times.Once);
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 40), Times.Once);
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 30), Times.Once);
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 20), Times.Once);
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 10), Times.Once);
        }
    }
}

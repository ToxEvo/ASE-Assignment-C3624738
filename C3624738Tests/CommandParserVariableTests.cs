using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738.Tests
{
    [TestClass]
    public class CommandParserVariableTests
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

        [TestMethod]
        public void ParseCommand_ExpressionsUsedToDrawConcentricCircles_CommandsExecutedCorrectly()
        {
            // Arrange
            string declareCount = "count = 5"; // You can set any value for count
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

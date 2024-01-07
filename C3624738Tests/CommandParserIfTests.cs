using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738.Tests
{
    [TestClass]
    public class CommandParserIfTests
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
        public void ParseCommand_IfStatement_TrueCondition_CommandExecuted()
        {
            // Arrange
            string ifCommand = "if 1 = 1\ncircle 20\nendif";

            // Act
            commandParser.ParseHandler("run", ifCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [TestMethod]
        public void ParseCommand_IfStatement_FalseCondition_CommandNotExecuted()
        {
            // Arrange
            string ifCommand = "if 1 = 2\ncircle 20\nendif";

            // Act
            commandParser.ParseHandler("run", ifCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}
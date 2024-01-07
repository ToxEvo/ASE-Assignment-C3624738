using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738.Tests
{
    /// <summary>
    /// Class to test the CommandParser loops.
    /// </summary>
    [TestClass]
    public class CommandParserLoopTests
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

        /// <summary>
        /// Test method for simple loop with integer as iterator and rectangle shape.
        /// </summary>
        [TestMethod]
        public void ParseCommand_SimpleLoop_RectangleCommandsExecutedCorrectly()
        {
            // Arrange
            string loopCommand = "loop 3\nrectangle 10 20\nendloop";

            // Act
            commandParser.ParseHandler("run", loopCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Test method for simple loop with integer as iterator and circle shape.
        /// </summary>
        [TestMethod]
        public void ParseCommand_SimpleLoop_CircleCommandsExecutedCorrectly()
        {
            // Arrange
            string loopCommand = "loop 4\ncircle 15\nendloop";

            // Act
            commandParser.ParseHandler("run", loopCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(4));
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Test method for loops with variable as iterator and radius for circle.
        /// </summary>
        [TestMethod]
        public void ParseCommand_LoopWithVariable_CircleCommandsExecutedCorrectly()
        {
            // Arrange
            string loopWithVariable = "loop x\ncircle x\nendloop";
            commandParser.ParseCommand("x = 3");

            // Act
            commandParser.ParseHandler("run", loopWithVariable);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        /// <summary>
        /// Test method for loops with variable as iterator.
        /// </summary>
        [TestMethod]
        public void ParseCommand_LoopWithVariable_CorrectNumberOfCircles()
        {
            // Arrange
            string loopWithVariable = "size = 3\nloop size\ncircle 20\nendloop";

            // Act
            commandParser.ParseHandler("run", loopWithVariable);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
        }
    }
}

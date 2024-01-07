using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Windows.Forms;

namespace C3624738.Tests
{
    /// <summary>
    /// Tests for the CommandParser class.
    /// </summary>
    [TestClass]
    public class CommandParserMethodTests
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
        /// Test for the ParseCommand method when calling a method without parameters.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CallMethodWithoutParameters_ShouldExecuteMethod()
        {
            // Arrange
            string defineMethod = "method drawShapes\ncircle 20\nrectangle 30 40\ncircle 10\nendmethod";
            string callMethod = "drawshapes";

            // Act
            commandParser.ParseHandler("run", defineMethod); // Define the method
            commandParser.ParseHandler("run", callMethod);  // Call the method

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(2));
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Test for the ParseCommand method when calling a method with parameters.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CallMethodWithParameters_ShouldExecuteMethodWithParameters()
        {
            // Arrange
            string defineMethod = "method drawShapes2 (x y)\ncircle x\nrectangle x y\ncircle x\nendmethod";
            string callMethod = "drawshapes2 (20 30)";

            // Act
            commandParser.ParseHandler("run", defineMethod); // Define the method
            commandParser.ParseHandler("run", callMethod);   // Call the method

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), 20), Times.Exactly(2));
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), 20, 30), Times.Once);
        }
    }
}

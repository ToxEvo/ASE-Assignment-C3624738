using Microsoft.VisualStudio.TestTools.UnitTesting;
using C3624738;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Moq;

namespace C3624738.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        [TestMethod()]
        public void ParseCommand_ExecutesPenColorCommandCorrectl()
        {
            // Arrange
            var mockGraphicsGen = new Mock<IGraphical>();
            var mockPictureBox = new Mock<PictureBox>();
            var commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);

            // Act
            commandParser.ParseCommand("pen red");

            // Assert
            mockGraphicsGen.Verify(g => g.SetColor(It.Is<(int, int, int, int)>(c =>
                c.Item1 == 255 && c.Item2 == 255 && c.Item3 == 0 && c.Item4 == 0)), Times.Once);
        }

        [TestMethod()]
        public void ParseHandler_ExecutesMultipleCommandsOnRun()
        {
            // Arrange
            var mockGraphicsGen = new Mock<IGraphical>();
            var mockPictureBox = new Mock<PictureBox>();
            var commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);

            string multiLineCommands = "pen red\n" + // Change pen color to red
                                       "position pen 100 100\n" + // Move pen to (100, 100)
                                       "circle 50\n" + // Draw a circle with radius 50
                                       "pen blue\n" + // Change pen color to blue
                                       "rectangle 200 100"; // Draw a rectangle with width 200 and height 100

            // Act
            commandParser.ParseHandler("run", multiLineCommands);

            // Assert
            // Verify if SetColor and other methods were called the expected number of times
            mockGraphicsGen.Verify(g => g.SetColor(It.IsAny<(int, int, int, int)>()), Times.Exactly(2));
            mockGraphicsGen.Verify(g => g.SetCoords(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            // Refresh should be called once after executing all commands
            mockPictureBox.Verify(g => g.Refresh(), Times.Exactly(6));
        }
    }
}
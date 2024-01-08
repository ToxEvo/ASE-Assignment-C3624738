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
    /// <summary>
    /// Represents a class for testing the CommandParser class with valid commands.
    /// </summary>
    [TestClass()]
    public class CommandParserTestsValid
    {
        private Mock<IGraphical> mockGraphicsGen;
        private Mock<PictureBox> mockPictureBox;
        private CommandParser commandParser;

        /// <summary> 
        /// Sets up the test environment before each test method is executed.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            // Arrange
            mockGraphicsGen = new Mock<IGraphical>();
            mockPictureBox = new Mock<PictureBox>();
            commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the PenColorCommand correctly.
        /// </summary>
        [TestMethod()]
        public void ParseCommand_ExecutesPenColorCommandCorrectly()
        {
            // Act
            commandParser.ParseCommand("pen red");

            // Assert
            mockGraphicsGen.Verify(g => g.SetColor(It.Is<(int, int, int, int)>(c =>
                c.Item1 == 255 && c.Item2 == 255 && c.Item3 == 0 && c.Item4 == 0)), Times.Once);
        }

        /// <summary>
        /// Tests the CommandParser class to ensure it loads commands from a file and executes them correctly.
        /// </summary>
        [TestMethod]
        public void CommandParser_LoadsCommandsFromFileAndExecutes()
        {
            // Arrange
            string tempFilePath = "C:\\Users\\toxev\\Source\\Repos\\ASE-Assignment-C3624738\\C3624738Tests\\TempFile\\test2.txt";
            string[] commandsToLoad = { "pen blue", "rectangle 100 200" };
            File.WriteAllLines(tempFilePath, commandsToLoad);

            // Act
            commandParser.ParseCommand("load C:\\Users\\toxev\\Source\\Repos\\ASE-Assignment-C3624738\\C3624738Tests\\TempFile\\ test2.txt");

            // Assert
            // Verify that the pen color was set to blue
            mockGraphicsGen.Verify(g => g.SetColor(It.Is<(int, int, int, int)>(c =>
                c.Item1 == 255 && c.Item2 == 0 && c.Item3 == 0 && c.Item4 == 255)), Times.Once);

            // Verify that a rectangle was drawn with the specified dimensions
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), 100, 200), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the PositionPenCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesPositionPenCommandCorrectly()
        {
            // Arrange
            int expectedX = 100;
            int expectedY = 200;

            // Act
            commandParser.ParseCommand($"position pen {expectedX} {expectedY}");

            // Assert
            mockGraphicsGen.Verify(g => g.SetCoords(expectedX, expectedY), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the PenDrawCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesPenDrawCommandCorrectly()
        {
            // Arrange
            int expectedX = 150;
            int expectedY = 250;

            // Act
            commandParser.ParseCommand($"pen draw {expectedX} {expectedY}");

            // Assert
            mockGraphicsGen.Verify(g => g.DrawTo(expectedX, expectedY), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the ClearCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesClearCommandCorrectly()
        {
            // Act
            commandParser.ParseCommand("clear");

            // Assert
            mockGraphicsGen.Verify(g => g.Clear(), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the ResetCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesResetCommandCorrectly()
        {
            // Act
            commandParser.ParseCommand("reset");

            // Assert
            mockGraphicsGen.Verify(g => g.SetCoords(0, 0), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the RectangleCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesRectangleCommandCorrectly()
        {
            // Arrange
            int expectedWidth = 200;
            int expectedHeight = 100;
            var initialPosition = (0, 0);

            // Act
            commandParser.ParseCommand($"rectangle {expectedWidth} {expectedHeight}");

            // Assert
            mockGraphicsGen.Verify(g => g.Rectangle(initialPosition.Item1, initialPosition.Item2, expectedWidth, expectedHeight), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the CircleCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesCircleCommandCorrectly()
        {
            // Arrange
            int expectedRadius = 50;

            // Get the initial position for the center of the circle (assuming it's (0, 0))
            var initialPosition = (0, 0);

            // Act
            commandParser.ParseCommand($"circle {expectedRadius}");

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(initialPosition.Item1, initialPosition.Item2, expectedRadius), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the FillOnCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesFillOnCommandCorrectly()
        {
            // Act
            commandParser.ParseCommand("fill on");

            // Assert
            mockGraphicsGen.Verify(g => g.SetFill(true), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method to ensure it executes the FillOffCommand correctly.
        /// </summary>
        [TestMethod]
        public void ParseCommand_ExecutesFillOffCommandCorrectly()
        {
            // Act
            commandParser.ParseCommand("fill off");

            // Assert
            mockGraphicsGen.Verify(g => g.SetFill(false), Times.Once);
        }
    }
}
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
    public class CommandParserTestsValid
    {
        private Mock<IGraphical> mockGraphicsGen;
        private Mock<PictureBox> mockPictureBox;
        private CommandParser commandParser;

        [TestInitialize]
        public void SetUp()
        {
            // Arrange
            mockGraphicsGen = new Mock<IGraphical>();
            mockPictureBox = new Mock<PictureBox>();
            commandParser = new CommandParser(mockGraphicsGen.Object, mockPictureBox.Object);
        }

        [TestMethod()]
        public void ParseCommand_ExecutesPenColorCommandCorrectly()
        {
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

            // Refresh should be called once after executing every command
            mockPictureBox.Verify(g => g.Refresh(), Times.Exactly(6));
        }

        [TestMethod]
        public void CommandParser_SavesCommandsToFile()
        {
            // Arrange
            string tempFilePath = "C:\\Users\\toxev\\Source\\Repos\\ASE-Assignment-C3624738\\C3624738Tests\\TempFile\\";
            string[] commandsToSave = { "pen red", "circle 50" };
            foreach (var cmd in commandsToSave)
            {
                commandParser.ParseCommand(cmd);
            }

            // Act
            commandParser.ParseCommand($"save {tempFilePath} test1.txt");

            // Assert
            var savedCommands = File.ReadAllLines("C:\\Users\\toxev\\Source\\Repos\\ASE-Assignment-C3624738\\C3624738Tests\\TempFile\\test1.txt");
            CollectionAssert.AreEqual(commandsToSave, savedCommands);
        }

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
    }
}
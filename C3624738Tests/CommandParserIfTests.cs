﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738.Tests
{
    /// <summary>
    /// Tests for the CommandParser If statements.
    /// </summary>
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

        /// <summary>
        /// Tests the ParseCommand method with an if statement and a true condition.
        /// The command should be executed.
        /// </summary>
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

        /// <summary>
        /// Tests the ParseCommand method with an if statement and a false condition.
        /// The command should not be executed.
        /// </summary>
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

        /// <summary>
        /// Tests the ParseCommand method with an if statement containing a multi-line block and a true condition.
        /// The commands should be executed.
        /// </summary>
        [TestMethod]
        public void ParseCommand_IfStatementWithMultiLineBlock_TrueCondition_CommandsExecuted()
        {
            // Arrange
            string ifCommand = "if 1 = 1\ncircle 20\nrectangle 30 40\nendif";

            // Act
            commandParser.ParseHandler("run", ifCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        /// <summary>
        /// Tests the ParseCommand method with an if statement containing a multi-line block and a false condition.
        /// The commands should not be executed.
        /// </summary>
        [TestMethod]
        public void ParseCommand_IfStatementWithMultiLineBlock_FalseCondition_CommandsNotExecuted()
        {
            // Arrange
            string ifCommand = "if 1 = 2\ncircle 20\nrectangle 30 40\nendif";

            // Act
            commandParser.ParseHandler("run", ifCommand);

            // Assert
            mockGraphicsGen.Verify(g => g.Circle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            mockGraphicsGen.Verify(g => g.Rectangle(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }
    }
}

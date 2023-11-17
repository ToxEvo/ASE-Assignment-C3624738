﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using C3624738;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Windows.Forms;

namespace C3624738.Tests
{
    [TestClass()]
    public class CommandParserTestsInvalid
    {
        // Arrange
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
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseCommand_ThrowsExceptionForInvalidCommand_Crcle()
        {
            // Act
            commandParser.ParseCommand("crcle 50");

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseCommand_ThrowsExceptionForInvalidCommand_Movto()
        {
            // Act
            commandParser.ParseCommand("movto 100,100");

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCommand_ThrowsArgumentExceptionForCircleWithInvalidParameters()
        {
            // Act
            commandParser.ParseCommand("circle x");

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCommand_ThrowsArgumentExceptionForMovetoWithTooFewParameters()
        {
            // Act
            commandParser.ParseCommand("pen draw 100");

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ParseCommand_ThrowsArgumentExceptionForDrawtoWithTooManyParameters()
        {
            // Act
            commandParser.ParseCommand("pen draw 100,100,100");

            // Assert is handled by ExpectedException
        }
    }
}
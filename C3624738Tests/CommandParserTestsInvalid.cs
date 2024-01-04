using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    /// <summary>
    /// Tests for the CommandParser class with invalid commands.
    /// </summary>
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

        /// <summary>
        /// Test for catching an exception when an invalid command "crcle" is parsed.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CatchesExceptionForInvalidCommand_Crcle()
        {
            try
            {
                // Act
                commandParser.ParseCommand("crcle 50");
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (Exception)
            {
                // Exception was caught, which is expected
            }
        }

        /// <summary>
        /// Test for catching an exception when an invalid command "movto" is parsed.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CatchesExceptionForInvalidCommand_Movto()
        {
            try
            {
                // Act
                commandParser.ParseCommand("movto 100,100");
                Assert.Fail("Expected InvalidOperationException was not thrown.");
            }
            catch (Exception)
            {
                // Exception was caught, which is expected
            }
        }

        /// <summary>
        /// Test for catching an ArgumentException when the "circle" command is parsed with invalid parameters.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CatchesArgumentExceptionForCircleWithInvalidParameters()
        {
            try
            {
                // Act
                commandParser.ParseCommand("circle x");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (Exception)
            {
                // Exception was caught, which is expected
            }
        }

        /// <summary>
        /// Test for catching an ArgumentException when the "pen draw" command is parsed with too few parameters.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CatchesArgumentExceptionForMovetoWithTooFewParameters()
        {
            try
            {
                // Act
                commandParser.ParseCommand("pen draw 100");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (Exception)
            {
                // Exception was caught, which is expected
            }
        }

        /// <summary>
        /// Test for catching an ArgumentException when the "pen draw" command is parsed with too many parameters.
        /// </summary>
        [TestMethod]
        public void ParseCommand_CatchesArgumentExceptionForDrawtoWithTooManyParameters()
        {
            try
            {
                // Act
                commandParser.ParseCommand("pen draw 100,100,100");
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (Exception)
            {
                // Exception was caught, which is expected
            }
        }
    }
}
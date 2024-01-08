using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Windows.Forms;

namespace C3624738.Tests
{
    /// <summary>
    /// Tests for Program behaviour.
    /// </summary>
    [TestClass]
    public class ProgramTests
    {
        /// <summary>
        /// Test method to check if both programs are running.
        /// </summary>
        [TestMethod]
        public void TestBothProgramsRunning()
        {
            // Arrange
            var sharedGraphicalGen = new Graphical();
            var mainForm1 = new MainForm(sharedGraphicalGen);
            var mainForm2 = new MainForm(sharedGraphicalGen);

            // Act
            Thread thread1 = null;
            Thread thread2 = null;

            var thread1Task = Task.Run(() =>
            {
                thread1 = new Thread(() => Application.Run(mainForm1));
                thread1.SetApartmentState(ApartmentState.STA);
                thread1.Start();
            });

            var thread2Task = Task.Run(() =>
            {
                thread2 = new Thread(() => Application.Run(mainForm2));
                thread2.SetApartmentState(ApartmentState.STA);
                thread2.Start();
            });

            Task.WaitAll(thread1Task, thread2Task); // Wait for both threads to start

            // Assert
            Assert.IsNotNull(thread1);
            Assert.IsTrue(thread1.IsAlive);
            Assert.IsNotNull(thread2);
            Assert.IsTrue(thread2.IsAlive);
        }
    }
}

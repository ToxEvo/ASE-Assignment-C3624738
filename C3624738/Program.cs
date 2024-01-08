using C3624738;
using System;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// The main program class.
/// </summary>
static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Shared graphical generator for forms
        var sharedGraphicalGen = new Graphical();

        // Creating two instances of MainForm with shared graphics generator
        var mainForm1 = new MainForm(sharedGraphicalGen);
        var mainForm2 = new MainForm(sharedGraphicalGen);

        // Subscribing UpdateGraphics method to the GraphicsUpdated event
        sharedGraphicalGen.GraphicsUpdated += mainForm1.UpdateGraphics;
        sharedGraphicalGen.GraphicsUpdated += mainForm2.UpdateGraphics;

        // Starting two threads to run each form in a separate thread
        var thread1 = new Thread(() => Application.Run(mainForm1));
        thread1.SetApartmentState(ApartmentState.STA);
        thread1.Start();

        var thread2 = new Thread(() => Application.Run(mainForm2));
        thread2.SetApartmentState(ApartmentState.STA);
        thread2.Start();
    }
}
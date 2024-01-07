using C3624738;
using System;
using System.Threading;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var sharedGraphicalGen = new Graphical();

        var mainForm1 = new MainForm(sharedGraphicalGen);
        var mainForm2 = new MainForm(sharedGraphicalGen);

        sharedGraphicalGen.GraphicsUpdated += mainForm1.UpdateGraphics;
        sharedGraphicalGen.GraphicsUpdated += mainForm2.UpdateGraphics;

        var thread1 = new Thread(() => Application.Run(mainForm1));
        thread1.SetApartmentState(ApartmentState.STA);
        thread1.Start();

        var thread2 = new Thread(() => Application.Run(mainForm2));
        thread2.SetApartmentState(ApartmentState.STA);
        thread2.Start();
    }
}
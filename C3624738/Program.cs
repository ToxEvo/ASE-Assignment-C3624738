namespace C3624738;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        // Shared graphical generator
        var sharedGraphicalGen = new Graphical();

        // Create the first MainForm instance
        var mainForm1 = new MainForm(sharedGraphicalGen);
        var thread1 = new Thread(() => Application.Run(mainForm1));
        thread1.SetApartmentState(ApartmentState.STA);

        // Create the second MainForm instance
        var mainForm2 = new MainForm(sharedGraphicalGen);
        var thread2 = new Thread(() => Application.Run(mainForm2));
        thread2.SetApartmentState(ApartmentState.STA);

        // Start both threads
        thread1.Start();
        thread2.Start();
    }
}

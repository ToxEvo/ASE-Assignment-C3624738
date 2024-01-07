using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C3624738
{
    public partial class MainForm : Form
    {
        private Graphical graphicalGen;
        private CommandParser parser;

        public MainForm(Graphical sharedGraphicalGen)
        {
            InitializeComponent();
            this.graphicalGen = sharedGraphicalGen; // Shared graphical generator
            parser = new CommandParser(graphicalGen, graphicsBox);
            SetupGraphicsBox();
        }

        private void SetupGraphicsBox()
        {
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(graphicalGen.GraphicalGen);
        }

        private void RunClick(object sender, EventArgs e)
        {
            // Execute commands in a new task and refresh graphics upon completion
            Task.Run(() => parser.ParseHandler(commandTextBox.Text, programTextBox.Text))
                .ContinueWith(t => Invoke(new Action(UpdateGraphics)));
        }

        private void SyntaxClick(object sender, EventArgs e)
        {
            // Execute commands in a new task and refresh graphics upon completion
            Task.Run(() => parser.ParseMultiple(programTextBox.Text))
                .ContinueWith(t => Invoke(new Action(UpdateGraphics)));
        }

        public void UpdateGraphics()
        {
            // Check if the update is called from a non-UI thread
            if (this.graphicsBox.InvokeRequired)
            {
                // Use Invoke to marshal the execution to the UI thread
                this.graphicsBox.Invoke(new Action(() => this.graphicsBox.Refresh()));
            }
            else
            {
                // Directly update the PictureBox if already on the UI thread
                this.graphicsBox.Refresh();
            }
        }

    }
}
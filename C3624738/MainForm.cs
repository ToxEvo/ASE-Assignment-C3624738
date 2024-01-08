using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C3624738
{
    /// <summary>
    /// Represents the main form of the application.
    /// </summary>
    public partial class MainForm : Form
    {
        private Graphical graphicalGen;
        private CommandParser parser;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        /// <param name="sharedGraphicalGen">The shared graphical generator used for rendering.</param>
        public MainForm(Graphical sharedGraphicalGen)
        {
            InitializeComponent();
            this.graphicalGen = sharedGraphicalGen; // Shared graphical generator
            parser = new CommandParser(graphicalGen, graphicsBox);
            SetupGraphicsBox();
        }

        /// <summary>
        /// Sets up the properties and event handlers for the graphics box.
        /// </summary>
        private void SetupGraphicsBox()
        {
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(graphicalGen.GraphicalGen);
        }

        /// <summary>
        /// Handles the Run button click event, executing commands and refreshing graphics.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void RunClick(object sender, EventArgs e)
        {
            // Execute commands in a new task and refresh graphics upon completion
            Task.Run(() => parser.ParseHandler(commandTextBox.Text, programTextBox.Text))
                .ContinueWith(t => Invoke(new Action(UpdateGraphics)));
        }

        /// <summary>
        /// Handles the Syntax button click event, checking syntax and refreshing graphics.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void SyntaxClick(object sender, EventArgs e)
        {
            // Execute commands in a new task and refresh graphics upon completion
            Task.Run(() => parser.ParseMultiple(programTextBox.Text))
                .ContinueWith(t => Invoke(new Action(UpdateGraphics)));
        }

        /// <summary>
        /// Refreshes the graphical content of the application.
        /// </summary>
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
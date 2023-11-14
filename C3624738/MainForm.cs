namespace C3624738
{
    /// <summary>
    /// The main form for the graphical drawing application.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// The instance responsible for generating graphical content.
        /// </summary>
        private Graphical graphicalGen;

        /// <summary>
        /// The parser that interprets and processes drawing commands.
        /// </summary>
        private CommandParser parser;

        /// <summary>
        /// Initializes a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            // Initialize the form's components and setup the graphics generator and command parser.
            InitializeComponent();
            graphicalGen = new Graphical();
            parser = new CommandParser(graphicalGen, graphicsBox);
            SetupGraphicsBox();
        }

        /// <summary>
        /// Configures the graphics box control used for drawing.
        /// </summary>
        private void SetupGraphicsBox()
        {
            // Set the background color and register the Paint event handler for the graphics box.
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(graphicalGen.GraphicalGen);
        }

        /// <summary>
        /// Event handler for the Run button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void RunClick(object sender, EventArgs e)
        {
            // Execute the commands when the Run button is clicked.
            ExecuteCommands();
        }

        /// <summary>
        /// Event handler for the Syntax button click event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A EventArgs that contains the event data.</param>
        private void SyntaxClick(object sender, EventArgs e)
        {
            // Execute the syntax check or batch commands when the Syntax button is clicked.
            ExecuteSyntax();
        }

        /// <summary>
        /// Executes the drawing commands entered in the command text box.
        /// </summary>
        private void ExecuteCommands()
        {
            // Parse and execute the command entered in the command text box using the command parser.
            parser.ParseHandler(commandTextBox.Text, programTextBox.Text);
        }

        /// <summary>
        /// Executes a batch of drawing commands from the program text box.
        /// </summary>
        private void ExecuteSyntax()
        {
            // Parse and execute multiple commands entered in the program text box using the command parser.
            parser.ParseMultiple(programTextBox.Text);
        }
    }
}

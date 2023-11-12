namespace C3624738
{
    public partial class MainForm : Form
    {
        CommandParser parser;
        public MainForm()
        {
            parser = new CommandParser();
            InitializeComponent();
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(parser.ExampleGraphics);
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            parser.ParseCommand(commandTextBox.Text);
        }

        private void SyntaxCheckButtonClick(object sender, EventArgs e)
        {
            parser.CommandSyntaxCheck(programTextBox.Text);
        }
    }
}
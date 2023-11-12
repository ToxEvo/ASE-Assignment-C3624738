namespace C3624738
{
    public partial class MainForm : Form
    {
        Graphical graphicalGen;
        CommandParser parser;

        public MainForm()
        {
            graphicalGen = new Graphical();
            parser = new CommandParser(graphicalGen);
            InitializeComponent();
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(graphicalGen.GraphicalGen);
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
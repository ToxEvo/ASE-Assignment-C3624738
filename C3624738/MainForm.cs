namespace C3624738
{
    public partial class MainForm : Form
    {
        Graphical graphicalGen;
        CommandParser parser;

        public MainForm()
        {
            graphicalGen = new Graphical();
            InitializeComponent();
            parser = new CommandParser(graphicalGen, graphicsBox);
            graphicsBox.BackColor = Color.Gray;
            graphicsBox.Paint += new PaintEventHandler(graphicalGen.GraphicalGen);
        }

        private void RunButtonClick(object sender, EventArgs e)
        {
            parser.ParseHandler(commandTextBox.Text);
        }

        private void SyntaxCheckButtonClick(object sender, EventArgs e)
        {
            parser.CommandSyntaxCheck(programTextBox.Text);
        }
    }
}
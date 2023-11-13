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

        private void RunClick(object sender, EventArgs e)
        {
            parser.ParseHandler(commandTextBox.Text);
        }

        private void SyntaxClick(object sender, EventArgs e)
        {
            parser.ParseMultiple(programTextBox.Text);
        }
    }
}
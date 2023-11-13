using System;
using System.Drawing;
using System.Windows.Forms;

namespace C3624738
{
    public partial class MainForm : Form
    {
        private Graphical graphicalGen;
        private CommandParser parser;

        public MainForm()
        {
            InitializeComponent();
            graphicalGen = new Graphical();
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
            ExecuteCommands();
        }

        private void SyntaxClick(object sender, EventArgs e)
        {
            ExecuteSyntax();
        }

        private void ExecuteCommands()
        {
            parser.ParseHandler(commandTextBox.Text, programTextBox.Text);
        }

        private void ExecuteSyntax()
        {
            parser.ParseMultiple(programTextBox.Text);
        }
    }
}

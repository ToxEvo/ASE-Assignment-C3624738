namespace C3624738
{
    public partial class MainForm : Form
    {
        CommandParser parser;
        public MainForm()
        {
            parser = new CommandParser();
            InitializeComponent();
            pictureBox.BackColor = Color.Gray;
            pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(parser.ExampleGraphics);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {
            // Placeholder for run button logic
        }

        private void SyntaxCheckButton_Click(object sender, EventArgs e)
        {
            // Placeholder for syntax check button logic 
        }

    }
}
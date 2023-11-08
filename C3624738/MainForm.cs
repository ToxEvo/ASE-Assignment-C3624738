namespace C3624738
{
    public partial class MainForm : Form
    {
        private TextBox? commandTextBox;
        private TextBox? programTextBox;
        private Button? runButton;
        private Button? syntaxCheckButton;
        private Panel? drawingPanel;

        public MainForm()
        {
            InitializeComponent();
            InitializeCustomControls();
        }

        private void InitializeCustomControls()
        {
            commandTextBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(200, 40),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical 
            };
            this.Controls.Add(commandTextBox);

            programTextBox = new TextBox
            {
                Location = new Point(10, 60),
                Size = new Size(200, 400),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(programTextBox);

            drawingPanel = new Panel
            {
                Location = new Point(220, 10),
                Size = new Size(300, 300),
                BorderStyle = BorderStyle.FixedSingle 
            };
            this.Controls.Add(drawingPanel);

            runButton = new Button
            {
                Text = "Run",
                Location = new Point(220, 10),
                Size = new Size(80, 40)
            };

            runButton.Click += RunButton_Click;
            Controls.Add(runButton);

            syntaxCheckButton = new Button
            {
                Text = "Syntax",
                Location = new Point(310, 10),
                Size = new Size(80, 40)
            };

            syntaxCheckButton.Click += SyntaxCheckButton_Click;
            Controls.Add(syntaxCheckButton);

            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Text = "C3624738 Graphics";
        }

        private void RunButton_Click(object? sender, EventArgs e)
        {
            // Placeholder for run button logic
        }

        private void SyntaxCheckButton_Click(object? sender, EventArgs e)
        {
            // Placeholder for syntax check button logic 
        }

    }
}
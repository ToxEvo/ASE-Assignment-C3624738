namespace C3624738;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;
    private TextBox commandTextBox;
    private TextBox programTextBox;
    private Button runButton;
    private Button syntaxCheckButton;
    private Panel drawingPanel;


    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "Form1";

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
            Location = new Point(10, 100),
            Size = new Size(200, 150),
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        this.Controls.Add(programTextBox);

        drawingPanel = new Panel
        {
            Location = new Point(220, 10),
            Size = new Size(370, 275),
            BorderStyle = BorderStyle.FixedSingle 
        };
        this.Controls.Add(drawingPanel);

        runButton = new Button
        {
            Text = "Run",
            Location = new Point(10, 55),
            Size = new Size(80, 40)
        };

        runButton.Click += RunButton_Click;
        Controls.Add(runButton);

        syntaxCheckButton = new Button
        {
            Text = "Syntax",
            Location = new Point(110, 55),
            Size = new Size(80, 40)
        };

        syntaxCheckButton.Click += SyntaxCheckButton_Click;
        Controls.Add(syntaxCheckButton);

        AutoScaleDimensions = new SizeF(6F, 13F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Text = "C3624738 Graphics";
    }

    #endregion
}

namespace C3624738
{
    class CommandParser 
    {
        public CommandParser()
        {

        }

        public void ParseLine(string line)
        {
            
        }

        public void ExampleGraphics(Object sender, System.Windows.Forms.PaintEventArgs gp)
        {
            System.Drawing.Graphics g = gp.Graphics;
            Pen pen = new Pen(Color.Black, 3);
            g.DrawLine(pen, 0, 0, 123, 123);
            pen.Dispose();
        }
    }
}
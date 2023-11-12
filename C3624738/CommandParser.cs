namespace C3624738
{
    class CommandParser 
    {
        public CommandParser()
        {

        }

        public void ParseCommand(string line)
        {
            
        }

        public void CommandSyntaxCheck(string syntax)
        {

        }

        public void ExampleGraphics(Object sender, PaintEventArgs gp)
        {
            Graphics g = gp.Graphics;
            Pen pen = new Pen(Color.Black, 3);
            g.DrawLine(pen, 0, 0, 123, 123);
            pen.Dispose();
        }
    }
}
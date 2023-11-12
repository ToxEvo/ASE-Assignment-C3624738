namespace C3624738
{
    interface IGraphical
    {
        public void SetColor((int, int, int, int) color);
        public (int, int, int, int) GetColor();
        public void SetWidth(float width);
        public float GetWidth();
    }

    class Graphical : IGraphical
    {
        (int, int, int, int) penColor;
        private Pen pen;

        public Graphical()
        {
            pen = new Pen(Color.Black, 3);
        }

        ~Graphical()
        {
            pen.Dispose();
        }

        public void GraphicalGen(object sender, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.DrawLine(pen, 0, 0, 100, 100);
        }

        public void SetColor((int, int, int, int) color)
        {
            pen.Color = Color.FromArgb(color.Item1, color.Item2, color.Item3, color.Item4);
        }

        public  (int, int, int, int) GetColor() 
        {
            Color color = pen.Color;
            return (color.A, color.R, color.G, color.B);
        }

        public void SetWidth(float width)
        {
            pen.Width = width;
        }

        public float GetWidth()
        {
            return pen.Width;
        }
    }
}
using System.Security.Cryptography.X509Certificates;

namespace C3624738
{
    interface IGraphical
    {
        public void SetColor((int, int, int, int) color);
        public (int, int, int, int) GetColor();
        public void SetWidth(float width);
        public float GetWidth();
        public void Circle(int x, int y, int radius);
        public void Clear();
        public bool GetFill();
        public void SetFill(bool fill);
        public (int, int) GetCoords();
        public void SetCoords(int x, int y);
    }

    class Graphical : IGraphical
    {
        private Pen pen;
        protected List<Shape> shapes;
        protected bool fill;
        protected int x, y;

        public Graphical()
        {
            pen = new Pen(Color.Black, 3);

            shapes = new List<Shape>();
            fill = false;
            x = 0;
            y = 0;
        }

        ~Graphical()
        {
            pen.Dispose();
        }

        public void GraphicalGen(object sender, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach (Shape shape in shapes)
            {
                shape.Paint(g);
            }
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

        public void Circle(int x, int y, int radius)
        {
            Circle circle = new Circle(pen.Color, x, y, pen.Width, fill, radius);
            shapes.Add(circle);
        }

        public void Circle(int radius)
        {
            Circle circle = new Circle(pen.Color, x, y, pen.Width, fill, radius);
            shapes.Add(circle);
        }

        public void Clear()
        {
            shapes.Clear();
        }

        public bool GetFill()
        {
            return fill;
        }

        public void SetFill(bool fill)
        {
            this.fill = fill;
        }

        public (int, int) GetCoords()
        {
            return(x, y);
        }

        public void SetCoords(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
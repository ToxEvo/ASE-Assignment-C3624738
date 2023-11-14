namespace C3624738
{
    interface IGraphical
    {
        public void SetColor((int, int, int, int) color);
        public (int, int, int, int) GetColor();
        public void Circle(int x, int y, int radius);
        public void Rectangle(int x, int y, int width, int height);
        public void Clear();
        public bool GetFill();
        public void SetFill(bool fill);
        public (int, int) GetCoords();
        void SetCoords(int x, int y);
        public void MoveTo (int x, int y);        
        public void DrawTo(int x, int y);
    }

    class Graphical : IGraphical
    {
        private Pen pen;
        protected List<Shape> shapes;
        protected bool fill;
        protected (int x, int y) penCoords;

        public Graphical()
        {
            pen = new Pen(Color.Black, 3);

            shapes = new List<Shape>();
            fill = false;
            penCoords = (0, 0);
        }

        ~Graphical()
        {
            pen.Dispose();
        }

        public void GraphicalGen(object? sender, PaintEventArgs pea)
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

        public void Circle(int x, int y, int radius)
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
            return penCoords;
        }
        public void SetCoords(int x, int y)
        {
            penCoords = (x, y);
        }

        public void MoveTo(int x, int y)
        {
            penCoords = (x, y);
        }

        public void Rectangle(int x, int y, int width, int height)
        {
            Rectangle rect = new Rectangle(pen.Color, x, y, pen.Width, fill, width, height);
            shapes.Add(rect);
        }

        public void DrawTo(int x, int y)
        {
            // Draw a line from current penCoords to the new coordinates
            Line line = new Line(pen.Color, penCoords.x, penCoords.y, x, y, pen.Width);
            shapes.Add(line);
            MoveTo(x, y); // Update current pen position
        }
    }
}
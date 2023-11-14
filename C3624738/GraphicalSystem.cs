
namespace C3624738
{
    /// <summary>
    /// Defines the graphical operations that can be performed.
    /// </summary>
    interface IGraphical
    {
        /// <summary>
        /// Sets the current drawing color.
        /// </summary>
        /// <param name="color">A tuple representing the ARGB color components.</param>
        void SetColor((int alpha, int red, int green, int blue) color);

        /// <summary>
        /// Retrieves the current drawing color.
        /// </summary>
        /// <returns>A tuple representing the ARGB color components.</returns>
        (int, int, int, int) GetColor();

        /// <summary>
        /// Draws a circle at the specified coordinates with the given radius.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        void Circle(int x, int y, int radius);

        /// <summary>
        /// Draws a rectangle at the specified coordinates with the given width and height.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The y-coordinate of the rectangle's top-left corner.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        void Rectangle(int x, int y, int width, int height);

        /// <summary>
        /// Clears all shapes from the drawing area.
        /// </summary>
        void Clear();

        /// <summary>
        /// Retrieves the current fill status for shapes.
        /// </summary>
        /// <returns>True if shapes are filled, otherwise false.</returns>
        bool GetFill();

        /// <summary>
        /// Sets the fill status for drawing shapes.
        /// </summary>
        /// <param name="fill">True to fill shapes, false to draw only their outlines.</param>
        void SetFill(bool fill);

        /// <summary>
        /// Gets the current position of the graphical cursor.
        /// </summary>
        /// <returns>The current coordinates as a tuple.</returns>
        (int, int) GetCoords();

        /// <summary>
        /// Sets the current position of the graphical cursor.
        /// </summary>
        /// <param name="x">The x-coordinate to move to.</param>
        /// <param name="y">The y-coordinate to move to.</param>
        void SetCoords(int x, int y);

        /// <summary>
        /// Moves the cursor to the specified location without drawing.
        /// </summary>
        /// <param name="x">The x-coordinate to move to.</param>
        /// <param name="y">The y-coordinate to move to.</param>
        void MoveTo(int x, int y);

        /// <summary>
        /// Draws a line to the specified coordinates from the current position.
        /// </summary>
        /// <param name="x">The x-coordinate to draw to.</param>
        /// <param name="y">The y-coordinate to draw to.</param>
        void DrawTo(int x, int y);
    }

    /// <summary>
    /// Implements IGraphical to provide drawing operations.
    /// </summary>
    class Graphical : IGraphical
    {
        private Pen pen;
        protected List<Shape> shapes;
        protected bool fill;
        protected (int x, int y) penCoords;

        /// <summary>
        /// Initializes a new instance of the Graphical class with default settings.
        /// </summary>
        public Graphical()
        {
            pen = new Pen(Color.Black, 3);
            shapes = new List<Shape>();
            fill = false;
            penCoords = (0, 0);
        }

        /// <summary>
        /// Finalizes the Graphical instance by disposing of pen resources.
        /// </summary>
        ~Graphical()
        {
            pen.Dispose();
        }

        /// <summary>
        /// The event handler for painting graphical shapes to the screen.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="pea">The PaintEventArgs containing event data.</param>
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
namespace C3624738
{
    /// <summary>
    /// Defines the graphical operations that can be performed.
    /// </summary>
    public interface IGraphical
    {
        /// <summary>
        /// Sets the current drawing color.
        /// </summary>
        /// <param name="color">A tuple representing the ARGB color components.</param>
        public void SetColor((int alpha, int red, int green, int blue) color);

        /// <summary>
        /// Retrieves the current drawing color.
        /// </summary>
        /// <returns>A tuple representing the ARGB color components.</returns>
        public (int alpha, int red, int green, int blue) GetColor();

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
        public void Clear();

        /// <summary>
        /// Retrieves the current fill status for shapes.
        /// </summary>
        /// <returns>True if shapes are filled, otherwise false.</returns>
        public bool GetFill();

        /// <summary>
        /// Sets the fill status for drawing shapes.
        /// </summary>
        /// <param name="fill">True to fill shapes, false to draw only their outlines.</param>
        public void SetFill(bool fill);

        /// <summary>
        /// Gets the current position of the graphical cursor.
        /// </summary>
        /// <returns>The current coordinates as a tuple.</returns>
        public (int x, int y) GetCoords();

        /// <summary>
        /// Sets the current position of the graphical cursor.
        /// </summary>
        /// <param name="x">The x-coordinate to move to.</param>
        /// <param name="y">The y-coordinate to move to.</param>
        public void SetCoords(int x, int y);

        /// <summary>
        /// Moves the cursor to the specified location without drawing.
        /// </summary>
        /// <param name="x">The x-coordinate to move to.</param>
        /// <param name="y">The y-coordinate to move to.</param>
        public void MoveTo(int x, int y);

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
    public class Graphical : IGraphical
    {
        private Pen pen;
        protected List<Shape> shapes;
        protected bool fill;
        protected (int x, int y) penCoords;

        /// <summary>
        /// Event triggered when the graphical content is updated.
        /// </summary>
        public event Action GraphicsUpdated;

        /// <summary>
        /// Method to invoke the <see cref="GraphicsUpdated"/> event.
        /// </summary>
        protected void OnGraphicsUpdated()
        {
            GraphicsUpdated?.Invoke();
        }

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

        /// <summary>
        /// Sets the color of the pen used for drawing.
        /// </summary>
        /// <param name="color">A tuple containing alpha, red, green, and blue color components.</param>
        public void SetColor((int alpha, int red, int green, int blue) color)
        {
            // Set the pen color using the Color.FromArgb method to convert ARGB values to a Color object.
            pen.Color = Color.FromArgb(color.alpha, color.red, color.green, color.blue);
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Gets the current color of the pen.
        /// </summary>
        /// <returns>A tuple containing alpha, red, green, and blue color components of the current pen color.</returns>
        public (int alpha, int red, int green, int blue) GetColor() 
        {
            // Retrieve the current color from the pen and return it as a tuple.
            Color color = pen.Color;
            return (color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Draws a circle at the specified location with the specified radius.
        /// </summary>
        /// <param name="x">The x-coordinate of the circle's center.</param>
        /// <param name="y">The y-coordinate of the circle's center.</param>
        /// <param name="radius">The radius of the circle.</param>
        public void Circle(int x, int y, int radius)
        {
            // Create a new Circle object and add it to the list of shapes.
            Circle circle = new Circle(pen.Color, x, y, pen.Width, fill, radius);
            shapes.Add(circle);
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Clears all shapes from the current graphics context.
        /// </summary>
        public void Clear()
        {
            // Remove all shapes from the list.
            shapes.Clear();
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Gets the current fill status for shapes.
        /// </summary>
        /// <returns>Whether shapes are currently being filled.</returns>
        public bool GetFill()
        {
            // Return the current fill status.
            return fill;
        }

        /// <summary>
        /// Sets the fill status for shapes.
        /// </summary>
        /// <param name="fill">Whether shapes should be filled.</param>
        public void SetFill(bool fill)
        {
            // Update the fill status.
            this.fill = fill;
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Retrieves the current coordinates of the pen.
        /// </summary>
        /// <returns>The current pen coordinates as a tuple.</returns>
        public (int x, int y) GetCoords()
        {
            // Return the current position of the pen.
            return penCoords;
        }

        /// <summary>
        /// Sets the pen coordinates to the specified location.
        /// </summary>
        /// <param name="x">The x-coordinate to set.</param>
        /// <param name="y">The y-coordinate to set.</param>
        public void SetCoords(int x, int y)
        {
            // Update the current position of the pen.
            penCoords = (x, y);
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Moves to a new location without drawing.
        /// </summary>
        /// <param name="x">The x-coordinate to move to.</param>
        /// <param name="y">The y-coordinate to move to.</param>
        public void MoveTo(int x, int y)
        {
            // Set the current pen coordinates to the new location without drawing a line.
            penCoords = (x, y);
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Draws a rectangle with the specified width and height at the specified location.
        /// </summary>
        /// <param name="x">The x-coordinate of the rectangle's top-left corner.</param>
        /// <param name="y">The y-coordinate of the rectangle's top-left corner.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public void Rectangle(int x, int y, int width, int height)
        {
            // Create a new Rectangle object and add it to the list of shapes.
            Rectangle rect = new Rectangle(pen.Color, x, y, pen.Width, fill, width, height);
            shapes.Add(rect);
            OnGraphicsUpdated();
        }

        /// <summary>
        /// Draws a line from the current pen position to the specified location.
        /// </summary>
        /// <param name="x">The x-coordinate to draw to.</param>
        /// <param name="y">The y-coordinate to draw to.</param>
        public void DrawTo(int x, int y)
        {
            // Create a new Line object representing the line from the current position to the specified coordinates and add it to the list of shapes.
            Line line = new Line(pen.Color, penCoords.x, penCoords.y, x, y, pen.Width);
            shapes.Add(line);
            // Update the pen position after drawing the line.
            MoveTo(x, y);
            OnGraphicsUpdated();
        }
    }
}
namespace C3624738
{
    /// <summary>
    /// Represents the abstract base class for all shapes that can be drawn.
    /// </summary>
    public abstract class Shape
    {
        /// <summary>
        /// The X-coordinate of the shape's position.
        /// </summary>
        protected int x;

        /// <summary>
        /// The Y-coordinate of the shape's position.
        /// </summary>
        protected int y;

        /// <summary>
        /// The pen used to outline the shape.
        /// </summary>
        protected Pen pen;

        /// <summary>
        /// Determines whether the shape is filled (true) or not (false).
        /// </summary>
        protected bool fill;

        /// <summary>
        /// The brush used to fill the shape.
        /// </summary>
        protected SolidBrush solid;

        /// <summary>
        /// Initializes a new instance of the Shape class using the specified properties.
        /// </summary>
        /// <param name="color">The color of the shape's outline and fill.</param>
        /// <param name="x">The X-coordinate of the shape's position.</param>
        /// <param name="y">The Y-coordinate of the shape's position.</param>
        /// <param name="width">The width of the shape's outline.</param>
        /// <param name="fill">Whether the shape is filled with the specified color.</param>
        protected Shape(Color color, int x, int y, float width, bool fill)
        {
            this.x = x;
            this.y = y;
            this.fill = fill;
            this.solid = new SolidBrush(color);
            this.pen = new Pen(color, width);
        }

        /// <summary>
        /// Destroys the Shape instance and disposes the Pen and SolidBrush objects.
        /// </summary>
        ~Shape()
        {
            pen.Dispose();
            solid.Dispose();
        }

        /// <summary>
        /// When overridden in a derived class, paints the shape on the provided graphics context.
        /// </summary>
        /// <param name="graphics">The graphics context to draw the shape on.</param>
        public abstract void Paint(Graphics graphics);

        /// <summary>
        /// Retrieves the position of the shape.
        /// </summary>
        /// <returns>A tuple with the X and Y coordinates of the shape.</returns>
        public (int, int) GetPosition()
        {
            return (x, y);
        }

        /// <summary>
        /// Sets the position of the shape to the specified X and Y coordinates.
        /// </summary>
        /// <param name="x">The X-coordinate to move the shape to.</param>
        /// <param name="y">The Y-coordinate to move the shape to.</param>
        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

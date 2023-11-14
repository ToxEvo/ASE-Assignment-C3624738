namespace C3624738
{
    /// <summary>
    /// Represents a circle with customizable position, size, and style.
    /// </summary>
    class Circle : Shape
    {
        /// <summary>
        /// The radius of the circle.
        /// </summary>
        protected int radius;

        /// <summary>
        /// Initializes a new instance of the Circle class with specified styling and radius.
        /// </summary>
        /// <param name="color">The color of the circle's border.</param>
        /// <param name="x">The X-coordinate of the center of the circle.</param>
        /// <param name="y">The Y-coordinate of the center of the circle.</param>
        /// <param name="width">The width of the circle's border.</param>
        /// <param name="fill">Specifies whether the circle is filled with the specified color.</param>
        /// <param name="radius">The radius of the circle.</param>
        public Circle(Color color, int x, int y, float width, bool fill, int radius) 
            : base(color, x, y, width, fill)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Paints the circle onto the specified Graphics object.
        /// </summary>
        /// <param name="graphics">The Graphics object to draw the circle on.</param>
        public override void Paint(Graphics graphics)
        {
            // Defines the bounding rectangle for the circle.
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x - radius, y - radius, radius * 2, radius * 2);

            // Fills the circle with color if 'fill' is true; otherwise, draws the outline.
            if (fill)
                graphics.FillEllipse(solid, rect); // Fills the circle with the specified brush.
            else
                graphics.DrawEllipse(pen, rect); // Draws the circle with the specified pen.
        }

        /// <summary>
        /// Sets the radius of the circle to the specified value.
        /// </summary>
        /// <param name="radius">The new radius of the circle.</param>
        public void SetRadius(int radius)
        {
            this.radius = radius;
        }

        /// <summary>
        /// Retrieves the current radius of the circle.
        /// </summary>
        /// <returns>The radius of the circle.</returns>
        public int GetRadius()
        {
            return radius;
        }
    }
}

namespace C3624738
{
    /// <summary>
    /// Represents a line that can be drawn on a graphics surface with a specified color and width.
    /// </summary>
    class Line : Shape
    {
        /// <summary>
        /// Gets the starting point of the line.
        /// </summary>
        private Point start;

        /// <summary>
        /// Gets the ending point of the line.
        /// </summary>
        private Point end;

        /// <summary>
        /// Initializes a new instance of the Line class with specified start and end points.
        /// </summary>
        /// <param name="color">The color of the line.</param>
        /// <param name="x1">The X-coordinate of the start point of the line.</param>
        /// <param name="y1">The Y-coordinate of the start point of the line.</param>
        /// <param name="x2">The X-coordinate of the end point of the line.</param>
        /// <param name="y2">The Y-coordinate of the end point of the line.</param>
        /// <param name="width">The width of the line.</param>
        public Line(Color color, int x1, int y1, int x2, int y2, float width)
            : base(color, x1, y1, width, false) // A line is not filled, hence 'fill' is set to false.
        {
            start = new Point(x1, y1);
            end = new Point(x2, y2);
        }

        /// <summary>
        /// Paints the line onto the specified Graphics object.
        /// </summary>
        /// <param name="graphics">The Graphics object to draw the line on.</param>
        public override void Paint(Graphics graphics)
        {
            // Draws a line from 'start' to 'end' using the 'pen' defined in the base class.
            graphics.DrawLine(pen, start, end);
        }
    }
}

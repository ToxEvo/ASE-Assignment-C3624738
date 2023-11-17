namespace C3624738
{
    /// <summary>
    /// Represents a rectangle shape with customizable position, size, and style attributes.
    /// </summary>
    public class Rectangle : Shape
    {
        /// <summary>
        /// The width of the rectangle.
        /// </summary>
        private int width;

        /// <summary>
        /// The height of the rectangle.
        /// </summary>
        private int height;

        /// <summary>
        /// Initializes a new instance of the Rectangle class with specified styling and dimensions.
        /// </summary>
        /// <param name="color">The color of the rectangle's border.</param>
        /// <param name="x">The X-coordinate of the rectangle's upper-left corner.</param>
        /// <param name="y">The Y-coordinate of the rectangle's upper-left corner.</param>
        /// <param name="penWidth">The width of the rectangle's border.</param>
        /// <param name="fill">Specifies whether the rectangle is filled with the specified color.</param>
        /// <param name="width">The width of the rectangle.</param>
        /// <param name="height">The height of the rectangle.</param>
        public Rectangle(Color color, int x, int y, float penWidth, bool fill, int width, int height)
            : base(color, x, y, penWidth, fill)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Paints the rectangle onto the specified Graphics object.
        /// </summary>
        /// <param name="graphics">The Graphics object to draw the rectangle on.</param>
        public override void Paint(Graphics graphics)
        {
            if (fill)
            {
                // Fill the rectangle with color using the SolidBrush instance.
                graphics.FillRectangle(solid, x, y, width, height);
            }
            else
            {
                // Draw only the rectangle's border using the Pen instance.
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }

        /// <summary>
        /// Sets the dimensions of the rectangle to the specified width and height.
        /// </summary>
        /// <param name="width">The new width of the rectangle.</param>
        /// <param name="height">The new height of the rectangle.</param>
        public void SetDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        /// <summary>
        /// Retrieves the current dimensions of the rectangle.
        /// </summary>
        /// <returns>A tuple containing the width and height of the rectangle.</returns>
        public (int Width, int Height) GetDimensions()
        {
            return (width, height);
        }
    }
}

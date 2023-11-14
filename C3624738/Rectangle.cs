namespace C3624738
{
    class Rectangle : Shape
    {
        private int width;
        private int height;

        // Constructor that accepts parameters for color, x, y, pen width, fill, width, and height
        public Rectangle(Color color, int x, int y, float penWidth, bool fill, int width, int height)
            : base(color, x, y, penWidth, fill)
        {
            this.width = width;
            this.height = height;
        }

        // Overridden Paint method to draw the rectangle
        public override void Paint(Graphics graphics)
        {
            if (fill)
            {
                // If the rectangle is to be filled, use SolidBrush
                graphics.FillRectangle(solid, x, y, width, height);
            }
            else
            {
                // If the rectangle is not filled, just draw the outline
                graphics.DrawRectangle(pen, x, y, width, height);
            }
        }

        // Method to set the dimensions of the rectangle
        public void SetDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        // Method to get the dimensions of the rectangle
        public (int, int) GetDimensions()
        {
            return (width, height);
        }
    }
}
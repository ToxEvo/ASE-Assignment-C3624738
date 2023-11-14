namespace C3624738
{
    class Circle : Shape
    {
        // Protected member for the radius.
        protected int radius;

        // Constructor that initializes a new instance of the Circle class.
        public Circle(Color color, int x, int y, float width, bool fill, int radius) 
            : base(color, x, y, width, fill)
        {
            this.radius = radius;
        }

        // Overridden Paint method to draw the circle.
        public override void Paint(Graphics graphics)
        {
            // Create a rectangle to bound the circle. This rectangle defines the circle's boundary.
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x - radius, y - radius, radius * 2, radius * 2);

            // Check whether the circle should be filled.
            if (fill)
                graphics.FillEllipse(solid, rect); // Use FillEllipse method to draw a filled circle.
            else
                graphics.DrawEllipse(pen, rect); // Use DrawEllipse method to draw an outline of the circle.
        }
    }
}
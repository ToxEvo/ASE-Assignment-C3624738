namespace C3624738
{
     class Circle : Shape
    {
        protected int radius;
        public Circle(Color colour, int x, int y, float penWidth, int radius) : base (colour, x, y, penWidth)
        {
            this.radius = radius;
        }
        public override void Paint(Graphics graphics)
        {
            Rectangle rect = new Rectangle(x - radius, y - radius, radius * 2, radius * 2);
            graphics.DrawEllipse(pen, rect);
        }
    }
}
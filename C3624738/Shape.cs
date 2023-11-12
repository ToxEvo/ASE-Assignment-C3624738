namespace C3624738
{
    abstract class Shape
    {
        protected int x, y;
        protected Pen pen;
        protected Shape(Color color, int x, int y, float width)
        {
            this.x = x;
            this.y = y;
            pen = new Pen(color, width);
        }
        ~Shape()
        {
            pen.Dispose();
        }
        public abstract void Paint(Graphics graphics);
        public (int, int) GetPosition()
        {
            return (x, y);
        }
        public void SetPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}
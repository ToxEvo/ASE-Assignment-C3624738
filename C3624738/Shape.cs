namespace C3624738
{
    abstract class Shape
    {
        protected int x, y;
        protected Pen pen;
        protected bool fill;
        protected SolidBrush solid;
        protected Shape(Color color, int x, int y, float width, bool fill)
        {
            this.x = x;
            this.y = y;
            this.fill = fill;
            solid = new SolidBrush(color);
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
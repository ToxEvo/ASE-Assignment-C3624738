namespace C3624738
{
    class Line : Shape
    {
        private Point start;
        private Point end;

        public Line(Color color, int x1, int y1, int x2, int y2, float width)
            : base(color, x1, y1, width, false) // 'false' since a line is not filled
        {
            start = new Point(x1, y1);
            end = new Point(x2, y2);
        }

        public override void Paint(Graphics graphics)
        {
            graphics.DrawLine(pen, start, end);
        }
    }
}

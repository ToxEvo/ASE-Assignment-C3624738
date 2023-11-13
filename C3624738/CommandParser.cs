namespace C3624738
{
    class CommandParser 
    {
        IGraphical graphicsGen;
        protected PictureBox graphicsBox;
        Dictionary<string, (int, int, int, int)> colors = new Dictionary<string, (int, int, int, int)>();

        public CommandParser(IGraphical graphicsGen, PictureBox graphicsBox)
        {
            this.graphicsGen = graphicsGen;
            this.graphicsBox = graphicsBox;
            colors.Add("red", (255, 255, 0, 0));
            colors.Add("green", (255, 0, 255, 0));
            colors.Add("blue", (255, 0, 0, 255));
            colors.Add("black", (255, 255, 255, 255));
        }

        private (int, int, int, int) FindColor(string color)
        {
            string readableColor = color.ToLower();
            if (colors.ContainsKey(readableColor))
            {
                return colors[readableColor];
            }
            else 
            {
                throw new Exception();
            }
        }
        public void ParseCommand(string command)
        {
            command = command.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
            string[] commands = command.Split(' ');

            switch (commands[0].ToLower())
            {
                case "pen":
                    (int, int, int, int) color = FindColor(commands[1]);
                    graphicsGen.SetColor(color);
                    break;
                case "circle":
                    if (commands.Length > 1 && int.TryParse(commands[1], out int radius))
                    {
                        // Assume x and y coordinates are obtained from the graphicsGen object.
                        (int, int) coords = graphicsGen.GetCoords();
                        graphicsGen.Circle(coords.Item1, coords.Item2, radius);
                    }
                    break;
                case "rectangle":
                    // Expecting command to be in the format "rectangle width height"
                    if (commands.Length == 3 
                        && int.TryParse(commands[1], out int width) 
                        && int.TryParse(commands[2], out int height))
                    {
                        // Assume x and y coordinates are obtained from the graphicsGen object.
                        (int, int) coords = graphicsGen.GetCoords();
                        graphicsGen.Rectangle(coords.Item1, coords.Item2, width, height);
                    }
                    else
                    {
                        throw new Exception("Invalid rectangle command format. Expected format: 'rectangle width height'");
                    }
                    break;
                case "clear":
                    if (commands.Length == 1)
                    {
                        graphicsGen.Clear();
                    }
                    break;
                case "fill":
                    if(commands.Length == 2) 
                    {
                        graphicsGen.SetFill(commands[1].Equals("on", StringComparison.OrdinalIgnoreCase));
                    }
                    break;
                     case "moveto":
                    if (commands.Length == 3 && int.TryParse(commands[1], out int moveToX) && int.TryParse(commands[2], out int moveToY))
                    {
                        graphicsGen.SetCoords(moveToX, moveToY);
                    }
                    else
                    {
                        throw new Exception("Invalid moveto command format. Expected format: 'moveto x y'");
                    }
                    break;
                default:
                    throw new Exception("Invalid command");
            }
        }

        public void ParseHandler(string line, string syntax)
        {
            if (line == "run")
            {
                ParseMultiple(syntax);
            }
            else
            {
                ParseCommand(line);
            }
            graphicsBox.Refresh();
        }

        public void ParseMultiple(string syntax)
        {
            foreach (string line in syntax.Split('\n'))
            {
                ParseCommand(line);
            }
            graphicsBox.Refresh();
        }
    }
}
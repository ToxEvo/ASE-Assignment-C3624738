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

            switch (commands[0])
            {
                case "pen":
                    (int, int, int, int) color = FindColor(commands[1]);
                    graphicsGen.SetColor(color);
                    break;
                case "circle":
                    if (int.TryParse(commands[1], out int x) && int.TryParse(commands[2], out int y)
                        && int.TryParse(commands[3], out int radius))
                    {
                        graphicsGen.Circle(x, y, radius);
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
                        if (commands[1] == "on")
                        {
                            graphicsGen.SetFill(true);
                        }
                        else if (commands[1] == "off")
                        {
                            graphicsGen.SetFill(false);
                        }
                    }
                    break;
                default:
                    throw new Exception("Invalid command");
            }
        }

        public void ParseHandler(string line)
        {
            ParseCommand(line);
            graphicsBox.Refresh();
        }

        public void CommandSyntaxCheck(string syntax)
        {
            foreach (string line in syntax.Split('\n'))
            {
                ParseCommand(line);
            }
            graphicsBox.Refresh();
        }
    }
}
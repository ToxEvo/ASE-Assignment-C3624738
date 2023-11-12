namespace C3624738
{
    class CommandParser 
    {
        IGraphical graphicsGen;
        Dictionary<string, (int, int, int, int)> colors = new Dictionary<string, (int, int, int, int)>();

        public CommandParser(IGraphical graphicsGen)
        {
            this.graphicsGen = graphicsGen;
            colors.Add("red", (255, 255, 0, 0));
            colors.Add("green", (255, 0, 255, 0));
            colors.Add("blue", (255, 0, 0, 255));
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
            string[] commands = command.Split(' ');

            switch (commands[0])
            {
                case "pen":
                (int, int, int, int) color = FindColor(commands[1]);
                graphicsGen.SetColor(color);
                break;
            }
        }

        public void CommandSyntaxCheck(string syntax)
        {
            foreach (string line in syntax.Split('\n'))
            {
                ParseCommand(line);
            }
        }
    }
}
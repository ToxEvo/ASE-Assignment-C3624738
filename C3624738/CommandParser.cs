namespace C3624738
{
    class CommandParser 
    {
        private readonly IGraphical graphicsGen;
        private readonly PictureBox graphicsBox;
        private readonly Dictionary<string, (int, int, int, int)> colors;
        private List<string> commandHistory = new List<string>();

        public CommandParser(IGraphical graphicsGen, PictureBox graphicsBox)
        {
            this.graphicsGen = graphicsGen ?? throw new ArgumentNullException(nameof(graphicsGen));
            this.graphicsBox = graphicsBox ?? throw new ArgumentNullException(nameof(graphicsBox));
            
            colors = new Dictionary<string, (int, int, int, int)>
            {
                {"red", (255, 0, 0, 255)},
                {"green", (0, 255, 0, 255)},
                {"blue", (0, 0, 255, 255)},
                {"black", (0, 0, 0, 255)},
                {"white", (255, 255, 255, 255)}
            };
        }

        private (int, int, int, int) FindColor(string color)
        {
            var readableColor = color.ToLower();
            if (colors.TryGetValue(readableColor, out var colorTuple))
            {
                return colorTuple;
            }
            throw new ArgumentException($"The color '{color}' is not defined.");
        }

        public void ParseCommand(string command)
        {
            // Trim and convert command to lowercase for checking against 'save' and 'load'
            string trimmedCommand = command.Trim().ToLower();

            // Avoid adding 'save' and 'load' commands to history to prevent recursion or redundant saving/loading
            if (!(trimmedCommand.StartsWith("save") || trimmedCommand.StartsWith("load")))
            {
                commandHistory.Add(command); // Add original command, not the trimmed/lowercase version
            }

            // Split the command into parts
            var commands = trimmedCommand.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                switch (commands[0].ToLower())
                {
                    case "pen":
                        ExecutePenCommand(commands);
                        break;
                    case "circle":
                        ExecuteCircleCommand(commands);
                        break;
                    case "rectangle":
                        ExecuteRectangleCommand(commands);
                        break;
                    case "clear":
                        ExecuteClearCommand(commands);
                        break;
                    case "fill":
                        ExecuteFillCommand(commands);
                        break;
                    case "position":
                        ExecutePositionCommand(commands);
                        break;
                    case "reset":
                        ExecuteResetCommand(commands);
                        break;
                    case "save":
                        ExecuteSaveCommand(commands);
                        break;
                    case "load":
                        ExecuteLoadCommand(commands);
                        break;
                    default:
                        throw new InvalidOperationException($"Unrecognized command: {commands[0]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExecutePenCommand(string[] commands)
        {
            if (commands.Length == 2)
            {
                var color = FindColor(commands[1]);
                graphicsGen.SetColor(color);
            }
            else if (commands.Length == 4 && commands[1].ToLower() == "draw")
            {
                if (int.TryParse(commands[2], out int x) && int.TryParse(commands[3], out int y))
                {
                    graphicsGen.DrawTo(x, y);
                }
                else
                {
                    throw new ArgumentException("Correct usage: 'pren color // pen draw x y'");
                }
            }
            else
            {
                throw new ArgumentException("Correct usage: 'pren color // pen draw x y'");
            }
        }

        private void ExecuteCircleCommand(string[] commands)
        {
            if (commands.Length != 2 || !int.TryParse(commands[1], out int radius))
                throw new ArgumentException("Correct usage: 'circle radius'");

            var coords = graphicsGen.GetCoords();
            graphicsGen.Circle(coords.Item1, coords.Item2, radius);
        }

        private void ExecuteRectangleCommand(string[] commands)
        {
            if (commands.Length != 3 || 
                !int.TryParse(commands[1], out int width) || 
                !int.TryParse(commands[2], out int height))
                throw new ArgumentException("Correct usage: 'rectangle width height'");

            var coords = graphicsGen.GetCoords();
            graphicsGen.Rectangle(coords.Item1, coords.Item2, width, height);
        }

        private void ExecuteClearCommand(string[] commands)
        {
            if (commands.Length > 1)
                throw new ArgumentException("Correct usage: 'clear'");
            graphicsGen.Clear();
        }

        private void ExecuteFillCommand(string[] commands)
        {
            if (commands.Length < 2 || !(commands[1].ToLower() == "on" || commands[1].ToLower() == "off"))
                throw new ArgumentException("Correct usage: 'fill on/off'");
            graphicsGen.SetFill(commands[1].ToLower() == "on");
        }

        private void ExecutePositionCommand(string[] commands)
        {
            if (commands.Length != 4 || commands[1].ToLower() != "pen")
                throw new ArgumentException("Correct usage: 'position pen x y'");
            if (!int.TryParse(commands[2], out int posX) || !int.TryParse(commands[3], out int posY))
                throw new ArgumentException("Correct usage: 'position pen x y'");
            
            graphicsGen.SetCoords(posX, posY);
        }

        private void ExecuteSaveCommand(string[] commands)
        {
            // Expecting command format: "save filepath filename"
            if (commands.Length != 3)
                throw new ArgumentException("Correct usage: 'save filepath filename'");

            string path = Path.Combine(commands[1], commands[2]);
            File.WriteAllLines(path, commandHistory);
        }

        private void ExecuteLoadCommand(string[] commands)
        {
            // Command format: "load filepath filename"
            if (commands.Length != 3)
                throw new ArgumentException("Correct usage: 'save filepath filename'");

            string fullPath = Path.Combine(commands[1], commands[2]);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"The file at '{fullPath}' was not found.");

            // Read all commands from the file and execute them except 'load' and 'save'
            string[] loadedCommands = File.ReadAllLines(fullPath);
            foreach (string loadedCommand in loadedCommands)
            {
                string trimmedCommand = loadedCommand.Trim().ToLower();
                // Skip 'load' and 'save' commands to prevent recursion and redundant saving
                if (trimmedCommand.StartsWith("load") || trimmedCommand.StartsWith("save"))
                    continue;

                // Parse and execute the command
                ParseCommand(loadedCommand);
            }
        }

        private void ExecuteResetCommand(string[] commands)
        {
            if (commands.Length > 1)
            {
                throw new ArgumentException("Correct usage: 'reset'");
            }
            graphicsGen.SetCoords(0, 0);
        }

        public void ParseHandler(string line, string syntax)
        {
            if (line == "run")
            {
                ParseMultiple(syntax);
            }
            else
            {
                ParseCommand(line.Trim());
            }
            graphicsBox.Refresh();
        }

        public void ParseMultiple(string syntax)
        {
            var lines = syntax.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                ParseCommand(line.Trim());
            }
            graphicsBox.Refresh();
        }
    }
}

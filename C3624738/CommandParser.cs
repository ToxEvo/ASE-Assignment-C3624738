namespace C3624738
{
    public class CommandParser 
    {
        /// <summary>
        /// Responsible for rendering graphics based on commands.
        /// </summary>
        private readonly IGraphical graphicsGen;
        
        /// <summary>
        /// The PictureBox control where graphics are displayed.
        /// </summary>
        private readonly PictureBox graphicsBox;

        /// <summary>
        /// Dictionary mapping color names to their RGBA values.
        /// </summary>
        private readonly Dictionary<string, (int, int, int, int)> colors;

        /// <summary>
        /// Stores the history of all commands executed, excluding 'save' and 'load' to prevent recursion.
        /// </summary>
        private List<string> commandHistory = new List<string>();

        /// <summary>
        /// Initializes a new instance of the CommandParser class with required graphical interface and picture box.
        /// </summary>
        /// <param name="graphicsGen">The graphical interface for executing commands.</param>
        /// <param name="graphicsBox">The picture box where drawings are rendered.</param>
        /// <exception cref="ArgumentNullException">Thrown when graphicsGen or graphicsBox is null.</exception>
        public CommandParser(IGraphical graphicsGen, PictureBox graphicsBox)
        {
            this.graphicsGen = graphicsGen ?? throw new ArgumentNullException(nameof(graphicsGen));
            this.graphicsBox = graphicsBox ?? throw new ArgumentNullException(nameof(graphicsBox));
            
            colors = new Dictionary<string, (int, int, int, int)>
            {
                {"red", (255, 255, 0, 0)},
                {"green", (255, 0, 255, 0)},
                {"blue", (255, 0, 0, 255)},
                {"white", (255, 255, 255, 255)},
                {"black", (255, 0, 0, 0)}
            };
        }

        /// <summary>
        /// Finds the RGBA value of the specified color.
        /// </summary>
        /// <param name="color">The name of the color to find.</param>
        /// <returns>The RGBA tuple of the color.</returns>
        /// <exception cref="ArgumentException">Thrown when the color is not defined.</exception>
        private (int, int, int, int) FindColor(string color)
        {
            var readableColor = color.ToLower();
            if (colors.TryGetValue(readableColor, out var colorTuple))
            {
                return colorTuple;
            }
            throw new ArgumentException($"The color '{color}' is not defined.");
        }

        /// <summary>
        /// Parses the input command and executes the corresponding graphical operation.
        /// </summary>
        /// <param name="command">The command to parse and execute.</param>
        /// <exception cref="InvalidOperationException">Thrown when the command is unrecognized.</exception>
        /// <exception cref="ArgumentException">Thrown when the command arguments are invalid.</exception>
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
                throw;
            }
        }

        /// <summary>
        /// Executes a command related to the pen.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name.</param>
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
                    throw new ArgumentException("Correct usage: 'pen color // pen draw x y'");
                }
            }
            else
            {
                throw new ArgumentException("Correct usage: 'pen color // pen draw x y'");
            }
        }

        /// <summary>
        /// Executes the circle drawing command.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name and the second is the radius.</param>
        private void ExecuteCircleCommand(string[] commands)
        {
            if (commands.Length != 2 || !int.TryParse(commands[1], out int radius))
                throw new ArgumentException("Correct usage: 'circle radius'");

            var coords = graphicsGen.GetCoords();
            graphicsGen.Circle(coords.Item1, coords.Item2, radius);
        }

        /// <summary>
        /// Executes the rectangle drawing command.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name, followed by width and height.</param>
        private void ExecuteRectangleCommand(string[] commands)
        {
            if (commands.Length != 3 || 
                !int.TryParse(commands[1], out int width) || 
                !int.TryParse(commands[2], out int height))
                throw new ArgumentException("Correct usage: 'rectangle width height'");

            var coords = graphicsGen.GetCoords();
            graphicsGen.Rectangle(coords.Item1, coords.Item2, width, height);
        }

        /// <summary>
        /// Executes the command to clear the drawing area.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name.</param>
        private void ExecuteClearCommand(string[] commands)
        {
            if (commands.Length > 1)
                throw new ArgumentException("Correct usage: 'clear'");
            graphicsGen.Clear();
        }

        /// <summary>
        /// Executes the fill command to toggle filling shapes on or off.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name and the second is the fill toggle.</param>
        private void ExecuteFillCommand(string[] commands)
        {
            if (commands.Length < 2 || !(commands[1].ToLower() == "on" || commands[1].ToLower() == "off"))
                throw new ArgumentException("Correct usage: 'fill on/off'");
            graphicsGen.SetFill(commands[1].ToLower() == "on");
        }

        /// <summary>
        /// Executes the command to set the pen position.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name, followed by the x and y coordinates.</param>
        private void ExecutePositionCommand(string[] commands)
        {
            if (commands.Length != 4 || commands[1].ToLower() != "pen")
                throw new ArgumentException("Correct usage: 'position pen x y'");
            if (!int.TryParse(commands[2], out int posX) || !int.TryParse(commands[3], out int posY))
                throw new ArgumentException("Correct usage: 'position pen x y'");
            
            graphicsGen.SetCoords(posX, posY);
        }

        /// <summary>
        /// Executes the save command to write command history to a file.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name, followed by the filepath and filename.</param>
        private void ExecuteSaveCommand(string[] commands)
        {
            // Expecting command format: "save filepath filename"
            if (commands.Length != 3)
                throw new ArgumentException("Correct usage: 'save filepath filename'");

            string path = Path.Combine(commands[1], commands[2]);
            File.WriteAllLines(path, commandHistory);
        }

        /// <summary>
        /// Executes the load command to read commands from a file and execute them.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name, followed by the filepath and filename.</param>
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

        /// <summary>
        /// Executes the reset command to set the pen coordinates to (0,0).
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name.</param>
        private void ExecuteResetCommand(string[] commands)
        {
            if (commands.Length > 1)
            {
                throw new ArgumentException("Correct usage: 'reset'");
            }
            graphicsGen.SetCoords(0, 0);
        }

        /// <summary>
        /// Handles parsing of a single line command or multiple lines of commands.
        /// </summary>
        /// <param name="line">The single line command to parse, or "run" to execute multiple commands.</param>
        /// <param name="syntax">The multi-line syntax containing commands to execute.</param>
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

        /// <summary>
        /// Parses and executes multiple lines of commands.
        /// </summary>
        /// <param name="syntax">The multi-line syntax containing commands to execute.</param>
        public void ParseMultiple(string syntax)
        {
            var lines = syntax.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                ParseCommand(line.Trim());
                graphicsBox.Refresh();
            }
        }
    }
}

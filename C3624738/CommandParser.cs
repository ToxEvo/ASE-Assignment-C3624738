using System.Data;

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

        private readonly Dictionary<string, int> variables = new Dictionary<string, int>();

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

            if (trimmedCommand.Contains("="))
            {
                HandleVariableDeclaration(trimmedCommand); // Ensure variable names are handled in lowercase
                return;
            }

            // Replace variables after splitting the command
            var commands = trimmedCommand.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < commands.Length; i++)
            {
                if (variables.ContainsKey(commands[i]))
                {
                    commands[i] = variables[commands[i]].ToString();
                }
            }

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

        private void HandleVariableDeclaration(string command)
        {
            var parts = command.Split('=');
            if (parts.Length == 2)
            {
                int value = EvaluateExpression(parts[1].Trim());
                variables[parts[0].Trim()] = value;
            }
            else
            {
                throw new ArgumentException("Invalid variable assignment");
            }
        }

        private int EvaluateExpression(string expression)
        {
            // Replacing variables with their values
            foreach (var variable in variables)
            {
                expression = expression.Replace(variable.Key, variable.Value.ToString());
            }

            // Evaluating the expression (basic implementation, can be improved)
            var result = new DataTable().Compute(expression, null);
            return Convert.ToInt32(result);
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
                throw new ArgumentException("Correct usage: 'load filepath filename'");

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
            bool inLoop = false, inIf = false;
            int loopCount = 0;
            string ifCondition = "";
            List<string> loopCommands = new List<string>();
            List<string> ifCommands = new List<string>();

            foreach (var line in lines)
            {
                string trimmedLine = line.Trim().ToLower();

                if (inLoop)
                {
                    if (trimmedLine == "endloop")
                    {
                        inLoop = false;
                        for (int i = 0; i < loopCount; i++)
                        {
                            foreach (var cmd in loopCommands)
                            {
                                ParseCommand(cmd); // Execute each command in the loop
                            }
                        }
                        loopCommands.Clear();
                    }
                    else
                    {
                        loopCommands.Add(line); // Accumulate loop commands
                    }
                }
                else if (inIf)
                {
                    if (trimmedLine == "endif")
                    {
                        inIf = false;
                        if (EvaluateCondition(ifCondition))
                        {
                            foreach (var cmd in ifCommands)
                            {
                                ParseCommand(cmd); // Execute each command in the if block
                            }
                        }
                        ifCommands.Clear();
                    }
                    else
                    {
                        ifCommands.Add(line); // Accumulate if commands
                    }
                }
                else if (trimmedLine.StartsWith("loop"))
                {
                    inLoop = true;
                    loopCount = GetLoopCount(trimmedLine);
                }
                else if (trimmedLine.StartsWith("if"))
                {
                    inIf = true;
                    ifCondition = ExtractCondition(trimmedLine);
                }
                else
                {
                    ParseCommand(trimmedLine); // Execute non-loop, non-if commands
                }

                graphicsBox.Refresh();
            }
        }


        private int GetLoopCount(string loopCommand)
        {
            // Extract the loop count or variable from the loop command
            string[] parts = loopCommand.Split(' ');
            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid loop syntax. Usage: loop [count]");
            }

            string loopCountStr = parts[1];
            if (int.TryParse(loopCountStr, out int loopCount))
            {
                return loopCount;
            }
            else if (variables.TryGetValue(loopCountStr, out loopCount))
            {
                return loopCount;
            }
            else
            {
                throw new ArgumentException("Loop count must be a number or a defined variable");
            }
        }

        private void ExecuteLoopCommands(List<string> loopCommands)
        {
            foreach (var command in loopCommands)
            {
                ParseCommand(command);
            }
        }

        private void ParseIfStatement(string[] lines, ref int index)
        {
            string condition = ExtractCondition(lines[index]);
            index++; // Move to the next line, which is the start of the if block

            List<string> ifBlockCommands = new List<string>();
            while (!lines[index].Trim().ToLower().Equals("endif"))
            {
                ifBlockCommands.Add(lines[index]);
                index++;
            }

            if (EvaluateCondition(condition))
            {
                foreach (var command in ifBlockCommands)
                {
                    ParseCommand(command); // Assuming you have a method to parse individual commands
                }
            }

            index++; // Skip the 'endif' line
        }

        private string ExtractCondition(string line)
        {
            return line.Substring(3).Trim(); // Extracts the condition part after 'if'
        }

        private bool EvaluateCondition(string condition)
        {
            // Implement the logic to evaluate the condition
            // This is a placeholder, actual implementation will depend on how you want to evaluate
            return Convert.ToBoolean(EvaluateExpression(condition)); // Use your existing EvaluateExpression method
        }

    }
}
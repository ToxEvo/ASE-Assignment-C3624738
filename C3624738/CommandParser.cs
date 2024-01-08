using System.Data;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace C3624738
{
    public class CommandParser
    {
        private Dictionary<string, MethodDefinition> methods = new Dictionary<string, MethodDefinition>();

        private class MethodDefinition
        {
            public List<string> Commands { get; set; } = new List<string>();
            public List<string> Parameters { get; set; } = new List<string>();
        }


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
        /// Dictionary holding variables and their values values.
        /// </summary>
        private readonly Dictionary<string, int> variables = new Dictionary<string, int>();

        private string currentMethod = null;

        /// <summary>
        /// Parses the given command and executes the corresponding action.
        /// </summary>
        /// <param name="command">The command to parse and execute.</param>
        public void ParseCommand(string command)
        {
            string trimmedCommand = command.Trim().ToLower();

            // Handling method definition
            if (currentMethod != null)
            {
                if (trimmedCommand == "endmethod")
                {
                    currentMethod = null;
                }
                else
                {
                    methods[currentMethod].Commands.Add(command);
                }
                return;
            }

            // Define a new method
            if (trimmedCommand.StartsWith("method "))
            {
                var match = Regex.Match(trimmedCommand, @"method (\w+)\s*(?:\((.*?)\))?");
                if (match.Success)
                {
                    currentMethod = match.Groups[1].Value;
                    var parameters = match.Groups[2].Value;
                    methods[currentMethod] = new MethodDefinition
                    {
                        Parameters = string.IsNullOrEmpty(parameters) ? new List<string>() : parameters.Split(' ').ToList()
                    };
                }
                return;
            }

            // Call a defined method
            var methodCallMatch = Regex.Match(trimmedCommand, @"(\w+)\s*(?:\((.*?)\))?");
            if (methodCallMatch.Success && methods.ContainsKey(methodCallMatch.Groups[1].Value))
            {
                var methodName = methodCallMatch.Groups[1].Value;
                var args = methodCallMatch.Groups[2].Value.Split(' ').ToList();
                CallMethod(methodName, args);
                return;
            }

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
                    case "endloop":
                        return;
                    default:
                        throw new InvalidOperationException($"Unrecognized command: {commands[0]}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CallMethod(string methodName, List<string> args)
        {
            var method = methods[methodName];

            // Save the current state of variables
            var originalVariables = new Dictionary<string, int>(variables);

            // Check if the method requires parameters
            if (method.Parameters.Count > 0)
            {
                if (args.Count != method.Parameters.Count)
                {
                    throw new ArgumentException($"Incorrect number of arguments for method {methodName}");
                }

                // Update variables with method parameters
                for (int i = 0; i < args.Count; i++)
                {
                    variables[method.Parameters[i]] = int.Parse(args[i]);
                }
            }

            // Execute each command in the method
            foreach (var cmd in method.Commands)
            {
                ParseCommand(cmd);
                graphicsBox.Refresh();
            }

            // Restore the original state of variables
            foreach (var kvp in originalVariables)
            {
                variables[kvp.Key] = kvp.Value;
            }
        }


        /// <summary>
        /// Handles the declaration of variables and evaluates expressions.
        /// </summary>
        /// <param name="command">The variable declaration command.</param>
        /// <exception cref="ArgumentException">Thrown when the variable assignment is invalid.</exception>
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

        /// <summary>
        /// Evaluates the given expression by replacing variables with their values and computing the result.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <returns>The result of the evaluated expression.</returns>
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

            SafeUIUpdate(() => {
                var coords = graphicsGen.GetCoords();
                graphicsGen.Circle(coords.Item1, coords.Item2, radius);
                graphicsBox.Refresh();
            });
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

            SafeUIUpdate(() => {
                var coords = graphicsGen.GetCoords();
                graphicsGen.Rectangle(coords.Item1, coords.Item2, width, height);
                graphicsBox.Refresh();
            });
        }

        /// <summary>
        /// Executes the command to clear the drawing area.
        /// </summary>
        /// <param name="commands">The command arguments where the first is the command name.</param>
        private void ExecuteClearCommand(string[] commands)
        {
            if (commands.Length > 1)
                throw new ArgumentException("Correct usage: 'clear'");

            SafeUIUpdate(() => {
                graphicsGen.Clear();
                graphicsBox.Refresh();
            });
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

        /// <summary>
        /// Handles the parsing and execution of a given line of syntax.
        /// </summary>
        /// <param name="line">The line of syntax to be parsed.</param>
        /// <param name="syntax">The complete syntax to be parsed when 'run' or 'syntax' is specified.</param>
        public void ParseHandler(string line, string syntax)
        {
            if (line == "run")
            {
                ParseMultiple(syntax);
            }
            else if (line == "syntax")
            {
                var errors = CheckSyntax(syntax);
                if (errors.Count > 0)
                {
                    // Each error message is separated by two new lines for clearer separation
                    string allErrors = string.Join("\n\n", errors);
                    MessageBox.Show(allErrors, "Syntax Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                ParseCommand(line.Trim());
            }
            graphicsBox.Refresh();
        }

        /// <summary>
        /// Checks the syntax of the provided script and returns a list of syntax errors.
        /// </summary>
        /// <param name="syntax">The script to check for syntax errors.</param>
        /// <returns>A list of syntax error messages.</returns>
        public List<string> CheckSyntax(string syntax)
        {
            var errors = new List<string>();
            var lines = syntax.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (!IsValidCommand(line))
                {
                    errors.Add($"Syntax error on line {i + 1}: '{line}' is not a valid command. Correct usage: " + GetCorrectUsageExample(line));
                }
            }

            return errors;
        }

        /// <summary>
        /// Provides a correct usage example for a given command.
        /// </summary>
        /// <param name="command">The command for which to provide usage example.</param>
        /// <returns>A string containing an example of correct usage.</returns>
        private string GetCorrectUsageExample(string command)
        {
            var commandType = command.Split(' ')[0].ToLower();
            switch (commandType)
            {
                case "pen":
                    return "'pen red' or 'pen draw 10 20'";
                case "circle":
                    return "'circle 10'";
                case "rectangle":
                    return "'rectangle 10 20'";
                case "clear":
                    return "'clear'";
                case "fill":
                    return "'fill on' or 'fill off'";
                case "position":
                    return "'position pen 10 20'";
                case "reset":
                    return "'reset'";
                case "save":
                    return "'save filepath filename'";
                case "load":
                    return "'load filepath filename'";
                case "method":
                    return "'method methodName (param1 param2) ... endmethod'";
                case "if":
                    return "'if condition ... endif'";
                case "loop":
                    return "'loop 5 ... endloop'";
                default:
                    return "No example available";
            }
        }

        /// <summary>
        /// Validates if the provided command is syntactically correct.
        /// </summary>
        /// <param name="command">The command to validate.</param>
        /// <returns>True if the command is valid, otherwise false.</returns>
        private bool IsValidCommand(string command)
        {
            // Example: Check if the command is empty
            if (string.IsNullOrWhiteSpace(command))
            {
                return false;
            }

            // Split the command into parts
            var parts = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Check if the command is recognized
            switch (parts[0].ToLower())
            {
                case "pen":
                case "circle":
                case "rectangle":
                case "clear":
                case "fill":
                case "position":
                case "reset":
                case "save":
                case "load":
                case "method":
                case "if":
                case "loop":
                    return ValidateSpecificCommand(parts);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Validates the specifics of a command based on its type.
        /// </summary>
        /// <param name="parts">The parts of the command split into an array.</param>
        /// <returns>True if the specific command is valid, otherwise false.</returns>
        private bool ValidateSpecificCommand(string[] parts)
        {
            switch (parts[0].ToLower())
            {
                case "pen":
                    // Validate pen command
                    return (parts.Length == 2 && colors.ContainsKey(parts[1])) ||
                           (parts.Length == 4 && parts[1].ToLower() == "draw" && int.TryParse(parts[2], out _) && int.TryParse(parts[3], out _));

                case "circle":
                    // Validate circle command
                    return parts.Length == 2 && int.TryParse(parts[1], out _);

                case "rectangle":
                    // Validate rectangle command
                    return parts.Length == 3 && int.TryParse(parts[1], out _) && int.TryParse(parts[2], out _);

                case "clear":
                    // Validate clear command
                    return parts.Length == 1;

                case "fill":
                    // Validate fill command
                    return parts.Length == 2 && (parts[1].ToLower() == "on" || parts[1].ToLower() == "off");

                case "position":
                    // Validate position command
                    return parts.Length == 4 && parts[1].ToLower() == "pen" && int.TryParse(parts[2], out _) && int.TryParse(parts[3], out _);

                case "reset":
                    // Validate reset command
                    return parts.Length == 1;

                case "save":
                case "load":
                    // Validate save and load commands
                    return parts.Length == 3;

                case "method":
                    // Validate method command
                    return parts.Length >= 2;

                case "if":
                    // Validate if command
                    return parts.Length >= 2;

                case "loop":
                    // Validate loop command
                    return parts.Length >= 2;

                default:
                    return false;
            }
        }


        /// <summary>
        /// Parses multiple commands based on the given syntax.
        /// </summary>
        /// <param name="syntax">The syntax containing multiple commands.</param>
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
                            foreach (var loopCommand in loopCommands)
                            {
                                ParseCommand(loopCommand);
                            }
                        }
                        loopCommands.Clear();
                    }
                    else
                    {
                        loopCommands.Add(line);
                    }
                }
                if (inIf)
                {
                    if (trimmedLine == "endif")
                    {
                        inIf = false;
                        if (EvaluateCondition(ifCondition))
                        {
                            foreach (var ifCommand in ifCommands)
                            {
                                ParseCommand(ifCommand);
                            }
                        }
                        ifCommands.Clear();
                    }
                    else
                    {
                        ifCommands.Add(line); // Store commands inside if block
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
                    SafeUIUpdate(() => {
                        ParseCommand(trimmedLine);
                        graphicsBox.Refresh();
                    });
                }
            }
        }


        /// <summary>
        /// Extracts the loop count or variable from the loop command.
        /// </summary>
        /// <param name="loopCommand">The loop command.</param>
        /// <returns>The loop count.</returns>
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

        /// <summary>
        /// Extracts the condition part after 'if'.
        /// </summary>
        /// <param name="line">The line containing the condition.</param>
        /// <returns>The extracted condition.</returns>
        private string ExtractCondition(string line)
        {
            return line.Substring(3).Trim();
        }

        /// <summary>
        /// Evaluates the given condition and returns the result.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>The result of the evaluation.</returns>
        private bool EvaluateCondition(string condition)
        {
            return Convert.ToBoolean(EvaluateExpression(condition));
        }

        /// <summary>
        /// Safely updates the UI from potentially non-UI threads.
        /// </summary>
        /// <param name="action">The action to be performed on the UI thread.</param>
        /// <remarks>
        /// This method checks if the call is made from a thread other than the one that created the graphicsBox control.
        /// If so, it uses Invoke to marshal the call to the UI thread.
        /// Otherwise, it executes the action directly.
        /// This ensures thread-safe operations on Windows Forms controls.
        /// </remarks>
        private void SafeUIUpdate(Action updateAction)
        {
            if (graphicsBox.InvokeRequired)
            {
                graphicsBox.Invoke(updateAction);
            }
            else
            {
                updateAction();
            }
        }


    }
}

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace C3624738
{
    class CommandParser 
    {
        private readonly IGraphical graphicsGen;
        private readonly PictureBox graphicsBox;
        private readonly Dictionary<string, (int, int, int, int)> colors;

        public CommandParser(IGraphical graphicsGen, PictureBox graphicsBox)
        {
            this.graphicsGen = graphicsGen ?? throw new ArgumentNullException(nameof(graphicsGen));
            this.graphicsBox = graphicsBox ?? throw new ArgumentNullException(nameof(graphicsBox));
            
            colors = new Dictionary<string, (int, int, int, int)>
            {
                {"red", (255, 0, 0, 255)},
                {"green", (0, 255, 0, 255)},
                {"blue", (0, 0, 255, 255)},
                {"black", (0, 0, 0, 255)}
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
            var commands = command.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

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
                    ExecuteResetCommand();
                    break;
                default:
                    throw new InvalidOperationException($"Unrecognized command: {commands[0]}");
            }
        }

        private void ExecutePenCommand(string[] commands)
        {
            if (commands.Length < 2)
                throw new ArgumentException("Not enough parameters for 'pen' command.");
            var color = FindColor(commands[1]);
            graphicsGen.SetColor(color);
        }

        private void ExecuteCircleCommand(string[] commands)
        {
            if (commands.Length < 2 || !int.TryParse(commands[1], out int radius))
                throw new ArgumentException("Invalid parameters for 'circle' command.");
            var coords = graphicsGen.GetCoords();
            graphicsGen.Circle(coords.Item1, coords.Item2, radius);
        }

        private void ExecuteRectangleCommand(string[] commands)
        {
            if (commands.Length < 3 || 
                !int.TryParse(commands[1], out int width) || 
                !int.TryParse(commands[2], out int height))
                throw new ArgumentException("Invalid parameters for 'rectangle' command.");
            var coords = graphicsGen.GetCoords();
            graphicsGen.Rectangle(coords.Item1, coords.Item2, width, height);
        }

        private void ExecuteClearCommand(string[] commands)
        {
            if (commands.Length > 1)
                throw new ArgumentException("Too many parameters for 'clear' command.");
            graphicsGen.Clear();
        }

        private void ExecuteFillCommand(string[] commands)
        {
            if (commands.Length < 2 || !(commands[1].ToLower() == "on" || commands[1].ToLower() == "off"))
                throw new ArgumentException("Invalid parameters for 'fill' command.");
            graphicsGen.SetFill(commands[1].ToLower() == "on");
        }

        private void ExecutePositionCommand(string[] commands)
        {
            if (commands.Length < 4 || 
                commands[1].ToLower() != "pen" || 
                !int.TryParse(commands[2], out int posX) || 
                !int.TryParse(commands[3], out int posY))
                throw new ArgumentException("Invalid parameters for 'position pen' command.");
            graphicsGen.SetCoords(posX, posY);
        }

        private void ExecuteResetCommand()
        {
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

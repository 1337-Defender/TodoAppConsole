using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace TodoAppConsole.Implementations
{
    public class ConsoleHandler
    {
        private enum AppState
        {
            MainMenu,
            HelpMenu,
            ManageTodos,
            ImportTodos,
            ExportTodos,
            EmailTodos,
            Quit
        }

        private AppState _currentState;
        private bool _running = true;

        public ConsoleHandler()
        {
            _currentState = AppState.MainMenu;
        }

        public void mainLoop()
        {
            while (_running)
            {
                switch(_currentState)
                {
                case AppState.MainMenu:
                        mainMenu();
                        break;
                    case AppState.HelpMenu:
                        displayHelp();
                        break;
                    case AppState.ManageTodos:
                        navigateToTasks();
                        break;
                    case AppState.ImportTodos:
                        // Placeholder for ImportTodos logic
                        break;
                    case AppState.ExportTodos:
                        // Placeholder for ExportTodos logic
                        break;
                    case AppState.EmailTodos:
                        // Placeholder for EmailTodos logic
                        break;
                    case AppState.Quit:
                        _running = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                    AnsiConsole.Clear();
            }
        }

        //private int getUserChoice(int cursorLeft, int cursorTop, Dictionary<string, string> inputs)
        //{
        //    bool selected = false;
        //    int option = 1;
        //    int numOptions = inputs.Count;
        //    Console.CursorVisible = false;

        //    while (!selected)
        //    {
        //        Console.SetCursorPosition(cursorLeft, cursorTop);

        //        int index = 1;
        //        foreach (KeyValuePair<string, string> pair in inputs)
        //        {
        //            Console.WriteLine($"{(index == option ? "\u001b[32m" : "")}{index}. {pair.Value} - {pair.Key}\u001B[0m");
        //            index++;
        //        }

        //        ConsoleKeyInfo key = Console.ReadKey(true);

        //        if (key.Key == ConsoleKey.UpArrow)
        //        {
        //            option = option == 1 ? numOptions : option - 1;
        //        }
        //        else if (key.Key == ConsoleKey.DownArrow)
        //        {
        //            option = option == numOptions ? 1 : option + 1;
        //        }
        //        else if (key.Key == ConsoleKey.Enter)
        //        {
        //            selected = true;
        //            break;
        //        }
        //    }

        //    Console.CursorVisible = true;
        //    return option - 1;
        //}

        /// <summary>
        /// @return
        /// </summary>
        public void displayHelp()
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void mainMenu()
        {
            AnsiConsole.Write(
                new FigletText("Todo App")
                .Centered()
                .Color(Color.Yellow)
            );

            var rule = new Rule("[yellow]Main Menu[/]");
            rule.Justification = Justify.Left;
            AnsiConsole.Write(rule);

            Dictionary<int, string> options = new Dictionary<int, string>()
            {
                { 1, "Help" },
                { 2, "Manage Todos" },
                { 3, "Import Todos" },
                { 4, "Export Todos" },
                { 5, "Email Todos" },
                { 6, "Quit" }
            };
            //int option = getUserChoice(Console.CursorLeft, Console.CursorTop, options);
            int option = AnsiConsole.Prompt(
                new SelectionPrompt<int>()
                .Title("Select an option:")
                .PageSize(10)
                .AddChoices(options.Keys)
                .UseConverter(key => $"{key}: {options[key]}")
            );

            switch (option)
            {
                case 2:
                    break;
                case 6:
                    _currentState = AppState.Quit;
                    break;
                default:
                    break;
            }
            return;
            
        }

        /// <summary>
        /// @return
        /// </summary>
        public void navigateToTasks()
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void navigateToCategories()
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void navigateToSettings()
        {
            // TODO implement here
            return;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void exitApplication()
        {
            // TODO implement here
            return;
        }
    }
}

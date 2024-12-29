using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

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
        private Layout _layout;
        private int _currentOption;
        public TaskManager TaskManager { get; private set; }

        public ConsoleHandler()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _currentState = AppState.MainMenu;
            _currentOption = 0;
            TaskManager = new TaskManager();
            TaskManager.addTask(
                "First task",
                "This is task 1 ka description",
                new DateTime(2024, 12, 25),
                new Category("Category 1"),
                "High"
            );
            TaskManager.addTask(
                "Second taskkkkkkkkkkkkkk",
                "This is task 2",
                new DateTime(2024, 11, 02),
                new Category("Category 1"),
                "Low"
            );
            TaskManager.addTask(
                "Third task",
                "Task 3 says hi!",
                new DateTime(2024, 06, 30),
                new Category("Category 2"),
                "Medium"
            );
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
                        showTasks();
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
                //AnsiConsole.Clear();
            }
        }

        private int getUserTodoChoice(int cursorLeft, int cursorTop, Dictionary<string, string> inputs)
        {
            bool selected = false;
            int option = 1;
            int numOptions = inputs.Count;
            Console.CursorVisible = false;

            while (!selected)
            {
                Console.SetCursorPosition(cursorLeft, cursorTop);

                int index = 1;
                foreach (KeyValuePair<string, string> pair in inputs)
                {
                    Console.WriteLine($"{(index == option ? "\u001b[32m" : "")}{index}. {pair.Value} - {pair.Key}\u001B[0m");
                    index++;
                }

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.UpArrow)
                {
                    option = option == 1 ? numOptions : option - 1;
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    option = option == numOptions ? 1 : option + 1;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    selected = true;
                    break;
                }
            }

            Console.CursorVisible = true;
            return option - 1;
        }

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
                    _currentState = AppState.ManageTodos;
                    break;
                case 6:
                    _currentState = AppState.Quit;
                    break;
                default:
                    break;
            }

            AnsiConsole.Clear();
            return;
            
        }

        private void InitializeLayout()
        {
            _layout = new Layout("Root")
                .SplitRows(
                    new Layout("Todos"),
                    new Layout("InfoPanel")
                        .SplitColumns(
                            new Layout("TodoInfo"),
                            new Layout("Controls")
                        )
                );

            _layout["Controls"].Update(BuildControlsPanel());
            // Set initial content
            UpdateTodosPanel();
            UpdateTodoInfoPanel();
        }

        private Panel BuildControlsPanel()
        {
            var controlsTable = new Table();
            controlsTable.AddColumn("Action");
            controlsTable.AddColumn("Key");

            controlsTable.AddRow("Navigate todos", "[bold gray]Up/Down[/]");
            controlsTable.AddRow("Navigate pages", "[bold gray]Left/Right[/]");
            controlsTable.AddRow("Mark task complete/incomplete", "[bold gray]Enter[/]");
            controlsTable.AddRow("Add todos", "[bold gray]a[/]");
            controlsTable.AddRow("Edit todos", "[bold gray]e[/]");
            controlsTable.AddRow("Exit", "[bold gray]Esc[/]");

            controlsTable.Columns[1].Centered();

            return new Panel(controlsTable)
                .Expand()
                .Header("[bold yellow]Controls[/]")
                .Border(BoxBorder.Double);
        }

        private void UpdateTodosPanel()
        {
            var todosTable = new Table();

            todosTable.AddColumns(["ID", "TITLE", "CATEGORY", "DUE DATE", "PRIORITY", "COMPLETED"]);
            foreach (var c in todosTable.Columns)
            {
                c.NoWrap();
            }
            todosTable.Columns[0].Width = 1;
            todosTable.Columns[5].Width = 6;

            int _ = 0;
            if (TaskManager.tasks.Count > 0)
            {
                foreach (var task in TaskManager.tasks)
                {
                    if (_currentOption == _)
                    {
                        todosTable.AddRow(
                            new Markup($"[invert]{task.id}[/]"),
                            new Markup($"[invert]{task.title}[/]"),
                            new Markup($"[invert]{task.category.name}[/]"),
                            new Markup($"[invert]{task.dueDate.ToShortDateString()}[/]"),
                            new Markup($"[invert]{task.priority}[/]"),
                            (task.isCompleted ? new Markup(":check_mark_button:") : new Markup(":cross_mark:"))
                        );
                    }
                    else
                    {
                        todosTable.AddRow(
                            new Markup($"{task.id}"),
                            new Markup($"{ task.title }"),
                            new Markup($"{task.category.name}"),
                            new Markup($"{task.dueDate.ToShortDateString()}"),
                            new Markup($"{task.priority}"),
                            (task.isCompleted ? new Markup(":check_mark_button:") : new Markup(":cross_mark:"))
                        );
                    }
                    _++;
                }
                _layout["Todos"].Update(
                new Panel(
                    Align.Center(
                        todosTable.Expand(),
                        VerticalAlignment.Top))
                    .Expand().Header(new PanelHeader("My [bold blue]Todos[/]"))
            );
            }
            else
            {
                _layout["Todos"].Update(
                new Panel(
                    Align.Center(
                        new Markup("[bold]No Todos Found![/]\n[red]Todos you add will appear here.[/]").Centered(),
                        VerticalAlignment.Middle))
                    .Expand().Header(new PanelHeader("My [bold blue]Todos[/]"))
            );
            }

            
        }

        private void UpdateTodoInfoPanel()
        {
            if (TaskManager.tasks.Count > 0)
            {
                _layout["TodoInfo"].Update(
                new Panel(
                Align.Center(
                        new Markup($"[bold green]Description[/]\n{TaskManager.tasks[_currentOption].description}"),
                        VerticalAlignment.Middle))
                    .Expand().Border(BoxBorder.Double).Header(new PanelHeader("[bold green]Todos[/] Info"))
                );
            }
            else
            {
                _layout["TodoInfo"].Update(
                new Panel(
                Align.Center(
                        new Markup("[bold red]No Todos Selected![/]"),
                        VerticalAlignment.Middle))
                    .Expand().Border(BoxBorder.Double).Header(new PanelHeader("[bold green]Todos[/] Info"))
                );
            }
        }

        public void showTasks()
        {
            Console.CursorVisible = false;

            // Initialize layout once
            if (_layout == null)
            {
                InitializeLayout();
            }

            

            // Render layout
            AnsiConsole.Clear();
            AnsiConsole.Write(_layout);

            // Process user input
            processTaskInput();

            Console.CursorVisible = false;
            AnsiConsole.Clear();
        }

        /// <summary>
        /// @return
        /// </summary>
        //public void showTasks()
        //{
        //    Console.CursorVisible = false;
        //    var layout = new Layout("Root")
        //    .SplitRows(
        //        new Layout("Todos"),
        //        new Layout("InfoPanel")
        //        .SplitColumns(
        //            new Layout("TodoInfo"),
        //            new Layout("Controls")
        //        )
        //    );

        //    layout["TodoInfo"].Ratio(2);
        //    layout["Controls"].Ratio(1);

        //    layout["Todos"].Update(
        //        new Panel(
        //            Align.Center(
        //                new Markup("My [blue]Todos![/]"),
        //                VerticalAlignment.Middle))
        //            .Expand()
        //    );
        //    layout["TodoInfo"].Update(
        //        new Panel(
        //            Align.Center(
        //                new Markup("Todo [red]Info![/]"),
        //                VerticalAlignment.Middle))
        //            .Expand()
        //    );

        //    var controlsTable = new Table();

        //    controlsTable.HideHeaders();
        //    controlsTable.HideFooters();
        //    //controlsTable.Border(TableBorder.None);

        //    controlsTable.AddColumn("Action");
        //    controlsTable.AddColumn("Key");

        //    controlsTable.AddRow("Mark task complete/incomplete", "[bold gray]Enter[/]");
        //    string vArrows = "↑ ↓";
        //    string hArrows = "← →";
        //    controlsTable.AddRow("Navigate todos", "[bold gray]Up/Down[/]");
        //    controlsTable.AddRow("Navigate pages", "[bold gray]Left/Right[/]");
        //    //controlsTable.AddRow("Navigate todos", new Markup("[bold gray]{0}[/]", vArrows));
        //    //controlsTable.AddRow("Navigate pages", new Markup("[bold gray]{0}[/]", hArrows));
        //    //controlsTable.AddRow("Navigate todos", "[bold gray]{0}[/]", Markup.Escape("↑ ↓"));
        //    //controlsTable.AddRow("Navigate pages", "[bold gray]{0}[/]", Markup.Escape("← →"));
        //    //controlsTable.AddRow("Navigate todos", "[bold gray]{\u2191 \u2193}[/]");
        //    //controlsTable.AddRow("Navigate pages", "[bold gray]{\u2190 \u2192}[/]");
        //    controlsTable.AddRow("Add todos", "[bold gray]a[/]");
        //    controlsTable.AddRow("Edit todos", "[bold gray]e[/]");

        //    controlsTable.Columns[1].Centered();

        //    var controlsPanel = new Panel(controlsTable.Centered().Expand());
        //    controlsPanel.Header = new PanelHeader("[bold yellow]Controls[/]");
        //    controlsPanel.Border = BoxBorder.Double;

        //    layout["Controls"].Update(
        //        controlsPanel.Expand()
        //    //new Panel(
        //    //    Align.Center(
        //    //        new Markup("Contr[yellow]ols![/]"),
        //    //        VerticalAlignment.Middle))
        //    //    .Expand()
        //    );

        //    AnsiConsole.Write(layout);

        //    ConsoleKeyInfo key = Console.ReadKey(true);

        //    AnsiConsole.Clear();
        //    return;
        //}

        private void processTaskInput()
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    _currentOption--;
                    if (_currentOption < 0)
                        _currentOption = 0;
                    break;
                case ConsoleKey.DownArrow:
                    _currentOption++;
                    if (_currentOption > TaskManager.tasks.Count - 1)
                        _currentOption = TaskManager.tasks.Count - 1;
                    break;
                case ConsoleKey.Escape:
                    _currentState = AppState.MainMenu;
                    break;
                case ConsoleKey.Enter:
                    TaskManager.tasks[_currentOption].isCompleted = !TaskManager.tasks[_currentOption].isCompleted;
                    break;
            }
            UpdateTodosPanel();
            UpdateTodoInfoPanel();
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

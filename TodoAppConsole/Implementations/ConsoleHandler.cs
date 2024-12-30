using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
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
        public FileSaver FileSaver { get; private set; }
        public CategoryManager CategoryManager { get; private set; }

        public ConsoleHandler()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _currentState = AppState.MainMenu;
            _currentOption = 0;

            FileSaver = new FileSaver();

            CategoryManager = new CategoryManager();

            TaskManager = new TaskManager();

            //TaskManager.saveData = FileSaver.loadFromFile(FileSaver.TodosFilePath);
            //TaskManager.tasks = TaskManager.saveData.Tasks;
            //TaskManager.nextId = TaskManager.saveData.nextId;
            TaskManager.tasks = FileSaver.TodosSaveData.Tasks;
            TaskManager.nextId = FileSaver.TodosSaveData.nextId;

            CategoryManager.categories = FileSaver.CategoriesSaveData.Categories;
            CategoryManager.nextId = FileSaver.CategoriesSaveData.nextId;

            //TaskManager.addTask(
            //    "First task",
            //    "This is task 1 ka description",
            //    new DateTime(2024, 12, 25),
            //    new Category("Category 1"),
            //    "High"
            //);
            //TaskManager.addTask(
            //    "Second taskkkkkkkkkkkkkk",
            //    "This is task 2",
            //    new DateTime(2024, 11, 02),
            //    new Category("Category 1"),
            //    "Low"
            //);
            //TaskManager.addTask(
            //    "Third task",
            //    "Task 3 says hi!",
            //    new DateTime(2024, 06, 30),
            //    new Category("Category 2"),
            //    "Medium"
            //);
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
            controlsTable.AddRow("Remove todos", "[bold gray]r[/]");
            controlsTable.AddRow("Manage Categories", "[bold gray]c[/]");
            controlsTable.AddRow("Exit", "[bold gray]Esc[/]");

            controlsTable.Columns[1].Centered();
            controlsTable.Expand().Centered();

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
                            new Markup($"[{task.category.color} invert]{task.id}[/]"),
                            new Markup($"[{task.category.color} invert]{task.title}[/]"),
                            new Markup($"[{task.category.color} invert]{task.category.name}[/]"),
                            new Markup($"[{task.category.color} invert]{task.dueDate.ToShortDateString()}[/]"),
                            new Markup($"[{task.category.color} invert]{task.priority}[/]"),
                            (task.isCompleted ? new Markup(":check_mark_button:") : new Markup(":cross_mark:"))
                        );
                    }
                    else
                    {
                        todosTable.AddRow(
                            new Markup($"[{task.category.color}]{task.id}[/]"),
                            new Markup($"[{task.category.color}]{ task.title }[/]"),
                            new Markup($"[{task.category.color}]{task.category.name}[/]"),
                            new Markup($"[{task.category.color}]{task.dueDate.ToShortDateString()}[/]"),
                            new Markup($"[{task.category.color}]{task.priority}[/]"),
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
                var rows = new Rows(
                    new Rule("[lime]Title[/]").LeftJustified(),
                    new Markup($"{TaskManager.tasks[_currentOption].title}"),
                    new Rule("[lime]Description[/]").LeftJustified(),
                    new Markup($"{TaskManager.tasks[_currentOption].description}")
                    );
                _layout["TodoInfo"].Update(
                //new Panel(
                //Align.Left(
                //        new Markup($"[bold green]Description[/]\n{TaskManager.tasks[_currentOption].description}"),
                //        VerticalAlignment.Middle))
                //    .Expand().Border(BoxBorder.Double).Header(new PanelHeader("[bold green]Todos[/] Info"))
                new Panel(
                Align.Left(
                        new Padder(new Panel(rows)).PadTop(1),
                        VerticalAlignment.Top))
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
                    .Expand().Border(BoxBorder.Double).Header(new PanelHeader("[bold lime]Todos[/] Info"))
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
                    TaskManager.toggeTaskCompletion(TaskManager.tasks[_currentOption].id, FileSaver);
                    //FileSaver.saveToFile(TaskManager.saveData);
                    //TaskManager.tasks[_currentOption].isCompleted = !TaskManager.tasks[_currentOption].isCompleted;
                    break;
                case ConsoleKey.A:
                    addTaskForm();
                    break;
                case ConsoleKey.E:
                    editTaskForm();
                    break;
                case ConsoleKey.R:
                    removeTaskForm();
                    break;
                case ConsoleKey.C:
                    navigateToCategories();
                    break;
            }
            UpdateTodosPanel();
            UpdateTodoInfoPanel();
        }

        public void addTaskForm()
        {
            AnsiConsole.Clear();
            Console.CursorVisible = true;

            var confirmation = AnsiConsole.Prompt(
                new ConfirmationPrompt("Add Task?"));

            if (!confirmation)
                return;

            AnsiConsole.Write(new Rule("[yellow]Title[/]"));
            string title = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter Todo [green]Title[/]: ")
            );

            AnsiConsole.Write(new Rule("[yellow]Description[/]"));
            string description = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter Todo [green]Description[/]: ")
            );

            AnsiConsole.Write(new Rule("[yellow]Category[/]"));
            var categories = CategoryManager.categories;

            var category = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                    .Title("Select a [green]category:[/]")
                    .UseConverter(c => $"ID: {c.id}, Name: {c.name}, Color: [{c.color}]{c.color}[/]")
                    .AddChoices(categories));
            AnsiConsole.Write(new Markup($"Category selected: [green]{category.name}[/]\n"));
            //string category = AnsiConsole.Prompt(
            //    new TextPrompt<string>("Enter Todo [green]Category[/]: ")
            //);

            AnsiConsole.Write(new Rule("[yellow]Due Date[/]"));
            var currDate = DateTime.Today;
            int year = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [green]Year[/]: ")
                .DefaultValue(currDate.Year)
            );
            int month = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [green]Month[/]: ")
                .DefaultValue(currDate.Month)
            );
            int day = AnsiConsole.Prompt(
                new TextPrompt<int>("Enter [green]Day[/]: ")
                .DefaultValue(currDate.Day)
            );
            var dueDate = new DateTime(year, month, day);

            AnsiConsole.Write(new Rule("[yellow]Priority[/]"));
            string priority = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter Todo [green]Priority[/]: ")
                .AddChoices(["Low", "Medium", "High"])
                .DefaultValue("Low")
            );

            TaskManager.addTask(title, description, dueDate, category, priority, FileSaver);
            //FileSaver.saveToFile(TaskManager.saveData);

            AnsiConsole.Write("[bold green]Task added successfully![/]");
            Console.CursorVisible = false;
        }

        public void editTaskForm()
        {
            AnsiConsole.Clear();
            Console.CursorVisible = true;

            var confirmation = AnsiConsole.Prompt(
                new ConfirmationPrompt("Edit Task?"));

            if (!confirmation)
                return;

            var task = TaskManager.tasks[_currentOption];

            AnsiConsole.Write(new Rule("[yellow]Title[/]"));
            AnsiConsole.Write(new Markup($"[aqua]Current Title: [/][dim]{task.title}[/]\n"));
            string title = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter new Todo [green]Title[/] [dim](Leave blank to keep unchanged)[/]: ")
                .AllowEmpty()
            );
            if (string.IsNullOrEmpty(title))
                title = task.title;

            AnsiConsole.Write(new Rule("[yellow]Description[/]"));
            AnsiConsole.Write(new Markup($"[aqua]Current Description: [/][dim]{task.description}[/]\n"));
            string description = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter Todo [green]Description[/] [dim](Leave blank to keep unchanged)[/]: ")
                .AllowEmpty()
            );
            if (string.IsNullOrEmpty(description))
                description = task.description;

            AnsiConsole.Write(new Rule("[yellow]Category[/]"));
            var categories = CategoryManager.categories;

            AnsiConsole.Write(new Markup($"[aqua]Current Category: [/][dim]{task.category.name}[/]\n"));
            var category = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                    .Title("Select a [green]category:[/]")
                    .UseConverter(c => $"ID: {c.id}, Name: {c.name}, Color: [{c.color}]{c.color}[/]")
                    .AddChoices(categories));
            AnsiConsole.Write(new Markup($"Category selected: [green]{category.name}[/]\n"));
            //string category = AnsiConsole.Prompt(
            //    new TextPrompt<string>("Enter Todo [green]Category[/] [dim](Leave blank to keep unchanged)[/]: ")
            //    .AllowEmpty()
            //);
            //if (string.IsNullOrEmpty(category))
            //    category = task.category.name;

            AnsiConsole.Write(new Rule("[yellow]Due Date[/]"));
            var editDateConfirmation = AnsiConsole.Prompt(
                new ConfirmationPrompt("Edit Due Date?"));

            DateTime dueDate;
            if (editDateConfirmation)
            {
                int year = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter [green]Year[/]: ")
                    .DefaultValue(task.dueDate.Year)
                );
                int month = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter [green]Month[/]: ")
                    .DefaultValue(task.dueDate.Month)
                );
                int day = AnsiConsole.Prompt(
                    new TextPrompt<int>("Enter [green]Day[/]: ")
                    .DefaultValue(task.dueDate.Day)
                );
                dueDate = new DateTime(year, month, day);
            }
            else
                dueDate = task.dueDate;

            AnsiConsole.Write(new Rule("[yellow]Priority[/]"));
            AnsiConsole.Write(new Markup($"[aqua]Current Priority: [/][dim]{task.priority}[/]\n"));
            string priority = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter Todo [green]Priority[/]: ")
                .AddChoices(["Low", "Medium", "High"])
                .DefaultValue(task.priority)
            );

            TaskManager.editTask(task.id, title, description, dueDate, category, priority, FileSaver);
            //task.title = title;
            //task.description = description;
            //task.priority = priority;
            //task.dueDate = dueDate;
            //task.category.name = category;

            //FileSaver.saveToFile(TaskManager.saveData);

            AnsiConsole.Write("[bold green]Task edited successfully![/]");
            Console.CursorVisible = false;
        }

        public void removeTaskForm()
        {
            AnsiConsole.Clear();
            Console.CursorVisible = true;

            var confirmationPrompt = new ConfirmationPrompt("[red]Remove Task?[/]");
            confirmationPrompt.DefaultValue = false;
            var confirmation = AnsiConsole.Prompt(
                confirmationPrompt
            );

            if (!confirmation)
                return;

            TaskManager.removeTask(TaskManager.tasks[_currentOption].id, FileSaver);
            //TaskManager.tasks.Remove(TaskManager.tasks[_currentOption]);
            //FileSaver.saveToFile(TaskManager.saveData);
            _currentOption = 0;

            AnsiConsole.Write("[bold green]Task removed successfully![/]");
            Console.CursorVisible = false;
        }

        /// <summary>
        /// @return
        /// </summary>
        public void navigateToCategories()
        {
            AnsiConsole.Clear();
            Console.CursorVisible = true;
            AnsiConsole.Write(new Rule("[yellow]Categories[/]"));

            bool _catRunning = true;
            while (_catRunning)
            {
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an [green]action[/]?")
                    .PageSize(5)
                    .AddChoices(new[] {
                        "View Categories", "Add Category", "Edit Category", "Remove Category", "Go back"
                    }));

                switch (choice)
                {
                    case "View Categories":
                        showCategoriesTable();
                        break;
                    case "Add Category":
                        addCategoriesForm();
                        break;
                    case "Edit Category":
                        editCategoriesForm();
                        break;
                    case "Remove Category":
                        removeCategoriesForm();
                        break;
                    case "Go back":
                        Console.CursorVisible = false;
                        _catRunning = false;
                        break;
                }
            }
        }

        public void showCategoriesTable()
        {
            var categoriesTable = new Table();

            categoriesTable.AddColumns(["ID", "NAME", "COLOUR"]);

            foreach (var category in CategoryManager.categories) 
            { 
                categoriesTable.AddRow(category.id.ToString(), category.name, category.color);
            }

            AnsiConsole.Write(categoriesTable);
        }

        public void addCategoriesForm()
        {
            var categories = CategoryManager.categories;

            AnsiConsole.Write(new Rule("[yellow]Name[/]"));
            var name = AnsiConsole.Prompt(
                new TextPrompt<string>("Enter the [green]name:[/]"));

            AnsiConsole.Write(new Rule("[yellow]Color[/]"));
            var colors = new Dictionary<string, string>
            {
                { "Red", "[red]█[/]" },
                { "Green", "[green]█[/]" },
                { "Blue", "[blue]█[/]" },
                { "Yellow", "[yellow]█[/]" },
                { "Cyan", "[cyan]█[/]" },
                { "Magenta", "[magenta]█[/]" },
                { "White", "[white]█[/]" },
                { "BrightGreen", "[lime]█[/]" }
            };

            var color = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a [green]color:[/]")
                    .AddChoices(colors.Keys)
                    .UseConverter(color => $"{color} {colors[color]}"));

            CategoryManager.createCategory(name, color.ToLower(), FileSaver);
            AnsiConsole.Clear();
        }

        public void editCategoriesForm()
        {
            var categories = CategoryManager.categories;

            var selectedCategory = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                    .Title("[yellow]Select a category to edit:[/]")
                    .UseConverter(c => $"ID: {c.id}, Name: {c.name}")
                    .AddChoices(categories));

            // Edit Name
            AnsiConsole.Write(new Rule("[yellow]Name[/]"));
            var newName = AnsiConsole.Prompt(
                new TextPrompt<string>("[green]Enter the new name:[/] [dim](Leave blank to keep unchanged)[/]")
                    .AllowEmpty());
            if (string.IsNullOrEmpty(newName))
                newName = selectedCategory.name;

            // Edit Color
            AnsiConsole.Write(new Rule("[yellow]Color[/]"));
            var colors = new Dictionary<string, string>
            {
                { "Red", "[red]█[/]" },
                { "Green", "[green]█[/]" },
                { "Blue", "[blue]█[/]" },
                { "Yellow", "[yellow]█[/]" },
                { "Cyan", "[cyan]█[/]" },
                { "Magenta", "[magenta]█[/]" },
                { "White", "[white]█[/]" },
                { "BrightGreen", "[lime]█[/]" }
            };

            var newColor = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[green]Select a new color:[/]")
                    .AddChoices(colors.Keys)
                    .UseConverter(color => $"{color} {colors[color]}"));

            CategoryManager.editCategory(selectedCategory.id, newName, newColor.ToLower(), FileSaver);
            AnsiConsole.Clear();
        }

        public void removeCategoriesForm()
        {
            var categories = CategoryManager.categories;

            var selectedCategory = AnsiConsole.Prompt(
                new SelectionPrompt<Category>()
                    .Title("[yellow]Select a form to edit:[/]")
                    .UseConverter(c => $"ID: {c.id}, Name: {c.name}")
                    .AddChoices(categories));

            var confirmationPrompt = new ConfirmationPrompt("[red]Remove Task?[/]");
            confirmationPrompt.DefaultValue = false;
            var confirmation = AnsiConsole.Prompt(
                confirmationPrompt
            );

            if (confirmation)
            {
                CategoryManager.deleteCategory(selectedCategory.id, FileSaver);
                AnsiConsole.Write("Task deleted [green]successfully![/]");
            }
            else
            {
                AnsiConsole.Write("Task deletion cancelled!");
            }
            AnsiConsole.Clear();
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

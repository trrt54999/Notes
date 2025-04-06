    using practice1_Batko_Daniel_KN24.Modules.Config;
    using practice1_Batko_Daniel_KN24.Modules.Shared;
    using practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;
    using practice1_Batko_Daniel_KN24.Modules.User;
    using Spectre.Console;

    namespace practice1_Batko_Daniel_KN24.Modules.Main.Menu;

    public class AdminMenu
    {
        private readonly ConfigService _configService = new();
        
        public void RenderMenu()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nYou are logged into the administration panel.\n");
            Console.ResetColor();

            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[bold green]Admin Menu:[/]")
                        .PageSize(5)
                        .AddChoices(new[] {
                            "List users",
                            "Sign Out",
                            "Exit program"
                        }));

                switch (choice)
                {
                    case "List users":
                        UserList();
                        break;
                    case "Sign Out":
                        SignOut();
                        break;
                    case "Exit program":
                        Console.Clear();
                        AnsiConsole.MarkupLine("[bold red]Exiting the program...[/]");
                        Environment.Exit(0);
                        break;
                    default:
                        AnsiConsole.MarkupLine("[bold red]Invalid option selected.[/]");
                        break;
                }
            }
        }

        public void ViewUserNotes()
        {
            string email = ConsoleUtils.ReadUserEmail("Enter user email: "); 
            string filePath = $"Data/DataBase/Notes/{email}.csv"; 

            if (!File.Exists(filePath)) 
            {
                Console.WriteLine($"No notes found for user with email: {email}");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                if (lines.Length <= 1)
                {
                    Console.WriteLine("This user has no notes.");
                    ConsoleUtils.AnyKey.Pause();
                    return;
                }

                var table = new Table();
                table.AddColumn("ID");
                table.AddColumn("Title");
                table.AddColumn("Content");
                table.AddColumn("Time");

                foreach (var line in lines.Skip(1))  
                {
                    var parts = line.Split(',');
                    table.AddRow(
                        Markup.Escape(parts[0]),
                        Markup.Escape(parts[1].Trim('"')),  
                        Markup.Escape(parts[2].Trim('"')),
                        Markup.Escape(parts[3].Trim('"'))
                    );
                }

                AnsiConsole.Render(table);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading notes: {ex.Message}");
            }

            ConsoleUtils.AnyKey.Pause();
        }
        
        public void UserList()
        {
            
            while (true)
            {
                var userService = new UserService();
                var users = userService.GetAll();
                
                var table = new Table();

                table.AddColumn("ID");
                table.AddColumn("Firstname");
                table.AddColumn("Lastname");
                table.AddColumn("Patronymic");
                table.AddColumn("Email");
                table.AddColumn("Role");

                foreach (var user in users)
                {
                    table.AddRow(
                        Markup.Escape(user.ID.ToString()),
                        Markup.Escape(user.FirstName),
                        Markup.Escape(user.LastName),
                        Markup.Escape(user.Patronymic),
                        Markup.Escape(user.Email),
                        Markup.Escape(user.Role)
                    );
                }
                AnsiConsole.Render(table);

                var action = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an action to perform on users:")
                        .PageSize(5)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices(new[] {
                            "View user notes",
                            "Remove user",
                            "Remove user notes",
                            "Back to main menu",
                            "Sign Out",
                            "Exit program",
                            "Readme Admin"
                        }));

                switch (action)
                {
                    case "View user notes":
                        ViewUserNotes();
                        break;
                    case "Remove user":
                        RemoveUser();
                        break;
                    case "Remove user notes":
                        RemoveUserNotes();
                        break;
                    case "Back to main menu":
                        return;
                    case "Sign Out":
                        SignOut();
                        break;
                    case "Exit program":
                        AnsiConsole.MarkupLine("[bold red]Exiting the program...[/]");
                        Environment.Exit(0); 
                        break;
                    case "Readme Admin":
                        AdminReadme();
                        break;
                    default:
                        throw new InvalidMenuOption();
                }
            }
        }
        
        public void AdminReadme()
        {
            string filePath = @"Data\Manual\README_Admin.txt";
            
            if (!File.Exists(filePath))
            {
                AnsiConsole.Markup("[red]Error: README_Admin.txt file not found.[/]\n");
                return;
            }

            try
            {
                string[] fileContent = File.ReadAllLines(filePath);
                
                AnsiConsole.Markup("[bold green]Admin README Content:\n[/]");
                foreach (var line in fileContent)
                {
                    AnsiConsole.MarkupLine(line);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[red]Error reading README_Admin.txt: {ex.Message}\n[/]");
            }

            ConsoleUtils.AnyKey.Pause();
        }
        
        public void RemoveUser()
        {
            int userId;
            while (true)
            {
                Console.Write("Enter the ID of the user to remove: ");
                string input = Console.ReadLine();
                
                if (!int.TryParse(input, out userId))
                {
                    Console.WriteLine("Error: Please enter a valid numeric ID.");
                    continue; 
                }

                break; 
            }

            AnsiConsole.Markup("[maroon]Are you sure you want to delete this user?[/] ([green]Y[/]/[red]N[/]): ");
            string confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation != "Y")
            {
                Console.WriteLine("User deletion canceled.");
                return;
            }

            try
            {
                var userService = new UserService();
                bool isDeleted = userService.Delete(userId);

                if (isDeleted)
                {
                    Console.WriteLine($"User with ID {userId} successfully removed.");
                }
                else
                {
                    Console.WriteLine($"User with ID {userId} not found or could not be removed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            ConsoleUtils.AnyKey.Pause();
        }


        public void RemoveUserNotes()
        {
            string email = ConsoleUtils.ReadUserEmail("Enter the email of the user whose notes you want to delete: ");

            string filePath = $"Data/DataBase/Notes/{email}.csv";

            if (!File.Exists(filePath))
            {
                AnsiConsole.Markup($"[maroon]No notes found for user with email:[/] {email}");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            AnsiConsole.Markup("[maroon]Are you sure you want to delete all notes for this user?[/] ([green]Y[/]/[red]N[/]): ");
            string confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation != "Y")
            {
                Console.WriteLine("Deletion of notes canceled.");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            try
            {
                File.Delete(filePath);
                Console.WriteLine($"Notes for user with email {email} have been successfully deleted.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting notes: {ex.Message}");
            }

            ConsoleUtils.AnyKey.Pause();
        }

        
        public void SignOut()
        {
            _configService.DeleteByKey(ConfigConstants.UserSession);
            Console.Clear();
            AnsiConsole.Markup("[red]Exiting the program...\n[/]");
            ConsoleUtils.AnyKey.Pause();
            App.Start();
        }
    }
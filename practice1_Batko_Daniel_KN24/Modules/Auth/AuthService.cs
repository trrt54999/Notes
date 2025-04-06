using practice1_Batko_Daniel_KN24.Modules.Config;
using practice1_Batko_Daniel_KN24.Modules.Main;
using practice1_Batko_Daniel_KN24.Modules.Shared;
using practice1_Batko_Daniel_KN24.Modules.User;
using practice1_Batko_Daniel_KN24.Modules.User.Entities;
using Spectre.Console;

namespace practice1_Batko_Daniel_KN24.Modules.Auth;

public class AuthService
{
    private const string FilePath = @"Data\DataBase\Users.csv";
    private string ReadmeFilePath = @"Data\Manual\README.txt"; 
    private readonly UserService _userService = new();
    private readonly ConfigService _configService = new();
    public void Login()
    {
        string email = ConsoleUtils.ReadUserEmail();

        if (!IsEmailExists(email))
        {
            AnsiConsole.Markup($"[underline red]{Constants.UserNotFound}\n[/]");
            return;
        }

        string password = ConsoleUtils.ReadUserInput("Enter your password: ");
        UserEntity? user = _userService.GetByEmail(email);

        while (user?.Password != password)
        {
            AnsiConsole.Markup($"[underline red]{Constants.InvalidLogin}\n[/]");
            Login();
            return;
        }

        var loginUser = (UserEntity)user;

        SaveUserSession(loginUser);

        Console.Clear();
        MenuFactory.RenderMenu(loginUser);
    }


    public bool IsEmailExists(string email)
    {
        string[] lines = File.ReadAllLines(FilePath);

        foreach (var line in lines.Skip(1))
        {
            try
            {
                var user = UserCsvParser.Parse(line);
                if (user.Email == email) return true;
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        return false;
    }
    
    public void Readme()
    {
        if (File.Exists(ReadmeFilePath))
        {
            try
            {
                string readmeContent = File.ReadAllText(ReadmeFilePath);
                AnsiConsole.Markup($"[white]{readmeContent}[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup($"[underline red]An error occurred while reading the file: {ex.Message}[/]\n");
            }
        }
        else
        {
            AnsiConsole.Markup("[underline red]File README.txt not found![/]\n");
        }
    }
    
    public void SignUp()
  {
    int ID = UserCsvParser.GenerateUserId(FilePath);
    string FirstName = ConsoleUtils.ReadUserInput("Enter your first name: ");

    while (!ValidationUtils.IsValidName(FirstName))
    {
        AnsiConsole.Markup("[underline red]Invalid first name! Please, try again!\n[/]");
        FirstName = ConsoleUtils.ReadUserInput("Enter your first name: ");
    }

    string LastName = ConsoleUtils.ReadUserInput("Enter your last name: ");

    while (!ValidationUtils.IsValidName(LastName))
    {
        AnsiConsole.Markup("[underline red]Invalid last name! Please, try again!\n[/]");
        LastName = ConsoleUtils.ReadUserInput("Enter your last name: ");
    }

    string Patronymic = ConsoleUtils.ReadUserInput("Enter your patronymic: ");

    while (!ValidationUtils.IsValidName(Patronymic))
    {
        AnsiConsole.Markup("[underline red]Invalid patronymic! Please, try again!\n[/]");
        Patronymic = ConsoleUtils.ReadUserInput("Enter your patronymic: ");
    }

    string Email = ConsoleUtils.ReadUserEmail();

    while (IsEmailExists(Email))
    {
        AnsiConsole.Markup("[underline red]The email has already been taken!\n[/]");
        Email = ConsoleUtils.ReadUserEmail();
    }

    string Password;
    string ConfirmPassword;

    while (true)
    {
        Password = ConsoleUtils.ReadUserInput("Enter your password: ");

        while (!ValidationUtils.IsValidPassword(Password))
        {
            AnsiConsole.Markup("[underline red]Password is too short or invalid! Please, try again!\n[/]");
            Password = ConsoleUtils.ReadUserInput("Enter your password: ");
        }

        ConfirmPassword = ConsoleUtils.ReadUserInput("Confirm your password: ");

        if (Password == ConfirmPassword)
        {
            break;
        }

        AnsiConsole.Markup("[underline red]Passwords do not match. Please enter your password again.\n[/]");
    }

    string Role = ConsoleUtils.ReadUserInput("Select role (User/Admin): ").ToLower();

    while (Role != "user" && Role != "admin")
    {
        AnsiConsole.Markup("[underline red]Invalid role! Please, try again.\n[/]");
        Role = ConsoleUtils.ReadUserInput("Select role (User/Admin): ").ToLower();
    }
    
    Role = char.ToUpper(Role[0]) + Role.Substring(1);
    
    AnsiConsole.Markup("[underline white][/]");

    try
    {
        var user = new UserEntity(ID, FirstName, LastName, Patronymic, Email, Password, Role);
        UserValidator.Validate(user);

        File.AppendAllText(FilePath, "\n" + user.GetCsvUser());
        AnsiConsole.Markup("[green]Registration successful!\n[/]");

        MenuFactory.RenderMenu(user);
    }
    catch (Exception ex)
    {
        AnsiConsole.Markup($"[underline red]An unexpected error occurred: {ex.Message}\n[/]");
    }
  }
    
    public void Exit()
    {
        AnsiConsole.Markup("[underline green]Exiting the application...[/]\n");
        Environment.Exit(0);
    }

    public UserEntity? GetAuthUser()
    {
        string configKey = ConfigConstants.UserSession;

        ConfigEntity? sessionConfig = _configService.GetByKey(ConfigConstants.UserSession);
        if (sessionConfig?.Key != configKey || string.IsNullOrEmpty(sessionConfig?.Value))
            return default;

        UserEntity? user = _userService.GetByEmail(((ConfigEntity)sessionConfig).Value);
        return user;
    }


    public bool IsAuthenticated()
    {
        var user = GetAuthUser();
        return user != null && ((UserEntity)user).IsValid();
    }
    
    private void SaveUserSession(UserEntity user)
    {
        var sessionConfig = new ConfigEntity(
            ConfigConstants.UserSession,
            user.Email,
            "Active user session",
            DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")
        );

        _configService.CreateOrUpdate(sessionConfig);
    }
}


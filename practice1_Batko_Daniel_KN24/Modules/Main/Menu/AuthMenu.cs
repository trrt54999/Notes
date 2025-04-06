using practice1_Batko_Daniel_KN24.Modules.Auth;
using practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;
using Spectre.Console;

namespace practice1_Batko_Daniel_KN24.Modules.Main.Menu;
    public class AuthMenu
    {
        private readonly AuthService authService;

        public AuthMenu()
        {
            authService = new AuthService();
        }

        public void RenderMenu()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an [green]option[/]:")
                           .PageSize(10)
                        .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                        .AddChoices("Exit", "Authorization", "Registration", "Manual")
                );

                switch (choice)
                {
                    case "Exit":
                        authService.Exit();
                        return;

                    case "Authorization":
                        authService.Login();
                        break;

                    case "Registration":
                        authService.SignUp();
                        break;

                    case "Manual":
                        authService.Readme();
                        break;
                    default:
                        throw new InvalidMenuOption();
                }
            }
        }
    }

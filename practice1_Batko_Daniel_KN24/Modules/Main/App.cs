using practice1_Batko_Daniel_KN24.Modules.Auth;
using practice1_Batko_Daniel_KN24.Modules.Main.Menu;
using practice1_Batko_Daniel_KN24.Modules.User.Entities;
using Spectre.Console;
using Color = Spectre.Console.Color;

namespace practice1_Batko_Daniel_KN24.Modules.Main;

public class App
{   
    private static readonly AuthService AuthService = new(); 
    public static void Start()
    {
        bool isAuth = AuthService.IsAuthenticated();
        
        AnsiConsole.Write(
            renderable: new FigletText("\nNOTES\n") // NOTES
                .LeftJustified()
                .Color(Color.Red));
        while (true)
        {
            if (!isAuth) new AuthMenu().RenderMenu();
         
            else
            {
                var user = AuthService.GetAuthUser();
                MenuFactory.RenderMenu((UserEntity)user!);
                Console.ReadKey();

            }
        }
    }
}
using practice1_Batko_Daniel_KN24.Modules.Main.Menu;
using practice1_Batko_Daniel_KN24.Modules.User.Entities;

namespace practice1_Batko_Daniel_KN24.Modules.Main;

public class  MenuFactory
{
    public static void RenderMenu(UserEntity user)
    {
        string role = user.Role;    
        switch (role)
        {
            case "User":
                new UserMenu().RenderMenu(user); 
                break;
            case "Admin":
                new AdminMenu().RenderMenu();
                break;
            default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid role! Acces denied.");
                Console.ResetColor();
                break;
        }
    }
}
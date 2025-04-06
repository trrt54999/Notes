using Spectre.Console;

namespace practice1_Batko_Daniel_KN24.Modules.Shared;

public class ConsoleUtils
{
    public static string ReadUserInput(string inputText = "Your choice: ", string defaultValue = "")
    {
        Console.Write(inputText);
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? defaultValue : input;
    }
    
    public static string ReadUserEmail(string inputText = "Enter your email: ", Func<string, bool> isEmailExists = null)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(inputText)
                .Validate(value =>
                {
                    if (!ValidationUtils.IsValidEmail(value))
                    {
                        return ValidationResult.Error("[red]Invalid email format! Please try again.[/]");
                    }

                    if (isEmailExists != null && isEmailExists(value))
                    {
                        return ValidationResult.Error("[red]This email is already taken! Please try a different one.[/]");
                    }

                    return ValidationResult.Success();
                })
        );
    }
    
    public static class AnyKey
    {
        public static void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            Console.Clear();
        }
    }
}
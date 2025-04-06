namespace practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;

public class InvalidMenuOption : Exception
{
    public InvalidMenuOption(string? message = "Error! Try to choose different option.") : base(message)
    {
    }
}
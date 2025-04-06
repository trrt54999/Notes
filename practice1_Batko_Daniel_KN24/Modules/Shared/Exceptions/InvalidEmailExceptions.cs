namespace practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;

class InvalidEmailException : Exception
{
    public InvalidEmailException(string? message = "Invalid email!") : base(message)
    {
    }
}
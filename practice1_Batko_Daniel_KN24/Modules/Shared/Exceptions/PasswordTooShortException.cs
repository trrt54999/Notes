namespace practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;

class PasswordTooShortException : Exception
{

    public PasswordTooShortException(string? message = "Password too short!") : base(message)
    {
    }
}
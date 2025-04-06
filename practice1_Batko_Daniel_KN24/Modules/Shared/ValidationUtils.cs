namespace practice1_Batko_Daniel_KN24.Modules.Shared;

public static class ValidationUtils
{
    
    public static bool IsValidEmail(string value)
    {
        if (!value.Contains('@') || !value.Contains('.')) return false;

        return true;
    }

    public static bool IsValidPassword(string userPassword)
    {
        return userPassword != null && userPassword.Length >= 6;
    }

    public static bool IsValidName(string name)
    {
        return name.Length > 1 && !name.Contains(',');
    }

    public static bool IsValidNotes(string notes)
    {
        return notes.Length > 2 && !notes.Contains("\"");
    }
}

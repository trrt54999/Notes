using practice1_Batko_Daniel_KN24.Modules.Shared;
using practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;

namespace practice1_Batko_Daniel_KN24.Modules.User.Entities;

public static class UserValidator
{
    public static void Validate(UserEntity user)
    {
        if (!ValidationUtils.IsValidEmail(user.Email))
            throw new InvalidEmailException();

        if (!ValidationUtils.IsValidPassword(user.Password))
            throw new PasswordTooShortException();
    }
}
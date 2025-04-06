using practice1_Batko_Daniel_KN24.Modules.Shared;
using practice1_Batko_Daniel_KN24.Modules.User.Entities;

namespace practice1_Batko_Daniel_KN24.Modules.User;

public class UserService
{
    private readonly CsvFileRepository<UserEntity> _userRepository =
        new(
            DatabaseConfig.UsersFilePath,
            UserCsvParser.Parse
        );

    public List<UserEntity> GetAll()
    {
        return _userRepository.GetAll();
    }

    public UserEntity? GetByEmail(string email)
    {
        return _userRepository.GetOneBy("Email", email);
    }

    public UserEntity Create(UserEntity user)
    {
        _userRepository.Create(user.ToCsv());

        // TODO: return new user, not the one from parameters
        return user;
    }

    public bool Delete(int id)
    {
        if (!File.Exists(DatabaseConfig.UsersFilePath))
        {
            throw new FileNotFoundException("Users file not found.");
        }

        string[] lines = File.ReadAllLines(DatabaseConfig.UsersFilePath);
        bool userFound = false;
        
        var updatedLines = lines.Where((line, index) =>
        {
            if (index == 0) return true;
            var parts = line.Split(',');
            if (int.TryParse(parts[0], out int currentId) && currentId == id)
            {
                userFound = true;
                return false;
            }
            return true;
        }).ToArray();
        
        if (userFound)
        {
            File.WriteAllLines(DatabaseConfig.UsersFilePath, updatedLines);
        }
        return userFound;
    }
}
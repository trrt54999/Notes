namespace practice1_Batko_Daniel_KN24.Modules.Shared;

public class DatabaseConfig
{
    public static readonly string UsersFilePath = @"Data/DataBase/Users.csv";
    public static readonly string ConfigFilePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data/Database/config.csv");
}
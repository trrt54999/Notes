namespace practice1_Batko_Daniel_KN24.Modules.User.Entities;

public struct UserEntity
{

    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }

    public string ToCsv()
    {
        return $"{ID},{FirstName},{LastName},{Patronymic},{Email},{Password},{Role}";
    }
    
    
    public UserEntity(int id, string firstName, string lastName, string patronymic, string email, string password,
        string role)
    {
        ID = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Patronymic = patronymic;
        Password = password;
        Role = role;
    }

    public string GetCsvUser()
    {
        return $"{ID},{FirstName},{LastName},{Patronymic},{Email},{Password},{Role}";
    }
    public bool IsValid()
    {
        return !String.IsNullOrEmpty(ID.ToString()) && !String.IsNullOrEmpty(Email); 
    }
}

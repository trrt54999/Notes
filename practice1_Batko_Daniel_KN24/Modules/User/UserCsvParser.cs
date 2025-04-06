    using practice1_Batko_Daniel_KN24.Modules.Shared;
    using practice1_Batko_Daniel_KN24.Modules.User.Entities;

    namespace practice1_Batko_Daniel_KN24.Modules.User;

    public class UserCsvParser
    {
        public static UserEntity Parse(string line) 
        {
            string[] parts = line.Split(',');

            int.TryParse(parts.ElementAtOrDefault(0), out var id);
            // TODO: handle errors
            var user = new UserEntity(
                 id,
                parts.ElementAtOrDefault(1) ?? Constants.DefaultValue,
                parts.ElementAtOrDefault(2) ?? Constants.DefaultValue,
                parts.ElementAtOrDefault(3) ?? Constants.DefaultValue,
                parts.ElementAtOrDefault(4) ?? Constants.DefaultValue,
                parts.ElementAtOrDefault(5) ?? Constants.DefaultValue,
                parts.ElementAtOrDefault(6) ?? Constants.DefaultValue
            );
            return user;
        }

        public static int GenerateUserId(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return 1;
            }

            string[] lines = File.ReadAllLines(filePath);
            
            if (lines.Length <= 1)
            {
                return 1;
            }

            int maxId = 0;
            
            foreach (var line in lines.Skip(1))
            {
                string[] parts = line.Split(',');
                if (int.TryParse(parts[0], out int currentId) && currentId > maxId)
                {
                    maxId = currentId;
                }
            }

            return maxId + 1;
        }
        
        
    }
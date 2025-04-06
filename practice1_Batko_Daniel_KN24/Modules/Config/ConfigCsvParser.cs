using practice1_Batko_Daniel_KN24.Modules.Shared;

namespace practice1_Batko_Daniel_KN24.Modules.Config;

public static class ConfigCsvParser
{
    public static ConfigEntity ParseFromCsv(string line)
    {
        string[] parts = line.Split(',');

        // Try parsing the ID
        int.TryParse(parts.ElementAtOrDefault(0), out var id);

        var configEntity = new ConfigEntity(
            parts.ElementAtOrDefault(1) ?? Constants.DefaultValue, 
            parts.ElementAtOrDefault(2) ?? Constants.DefaultValue,
            parts.ElementAtOrDefault(3) ?? Constants.DefaultValue,
            parts.ElementAtOrDefault(4) ?? Constants.DefaultValue
        );

        configEntity.Id = id;
        return configEntity;
    }
}
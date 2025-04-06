using practice1_Batko_Daniel_KN24.Modules.Shared;

namespace practice1_Batko_Daniel_KN24.Modules.Config;

public class ConfigService
{
    private readonly CsvFileRepository<ConfigEntity> _configRepository =
        new(
            DatabaseConfig.ConfigFilePath,
            ConfigCsvParser.ParseFromCsv
        );

    public ConfigEntity? GetByKey(string value)
    {
        var val = _configRepository.GetOneBy("Key", value);
        return val;
    }

    public void CreateOrUpdate(ConfigEntity config)
    {
        ConfigEntity configEntity = _configRepository.GetOneBy("Key", config.Key);
        
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        config.LastUpdated = currentTime;

        if (!configEntity.IsValid())
        {
            _configRepository.Create(config.ToCsv());
        }
        else
        {
            configEntity.LastUpdated = currentTime;
            _configRepository.Update(configEntity.Id, configEntity.ToDictionary());
        }
    }


    public void DeleteByKey(string key)
    {
        var configEntity = _configRepository.GetOneBy("Key", key);
        if (!configEntity.IsValid())
        {
            Console.WriteLine("Entity with given key not found: " + key);
            return;
        }

        _configRepository.DeleteById(configEntity.Id);
    }
}
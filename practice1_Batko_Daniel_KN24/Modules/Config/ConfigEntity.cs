namespace practice1_Batko_Daniel_KN24.Modules.Config;

public struct ConfigEntity(string key, string value, string description = "-", string lastUpdated = "-")
{
    public int Id { get; set; }
    public string Key { get; } = key;
    public string Value { get; } = value;
    public string Description { get; set; } = description;
    public string LastUpdated { get; set; } = lastUpdated;  

    public string ToCsv()
    {
        return  $"{Id},{Key},{Value},{Description},{LastUpdated}";
    }

    public Dictionary<string, string> ToDictionary()
    {
        return new Dictionary<string, string>
        {
            { "Id", Id.ToString() },
            { "Key", Key },
            { "Value", Value },
            { "Description", Description },
            {"LastUpdated", LastUpdated}
        };
    }
    
    public bool IsValid()
    {
        return !String.IsNullOrEmpty(Id.ToString())
               && !String.IsNullOrEmpty(Key)
               && !String.IsNullOrEmpty(Value);
    }
}
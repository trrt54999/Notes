namespace practice1_Batko_Daniel_KN24.Modules.Shared;

public class CsvFileRepository<T>(string filePath, Func<string, T> lineParser)
{
    // read everything from .csv file and return in format array in ArrayList
    public List<T> GetAll(bool includeHeader = false)
    {
        return GenericGetAll(lineParser, includeHeader);
    }

    // read everything from file and return as unparsed line 
    public List<string> GetAllRaw(bool includeHeader = false)
    {
        // it's just return a string value, which is a one row from .csv file
        return GenericGetAll((value) => value, includeHeader);
    }

    /**
     * Return all data from file in array of string inside the List
     */
    public List<string[]> GetAllAsArray(bool includeHeader = false)
    {
        return GenericGetAll((value) => value.Split(','), includeHeader);
    }

    public List<T> GetBy(string key, string value)
    {
        var list = GetAll();
        var result = new List<T>();

        foreach (var item in list)
        {
            var property = typeof(T).GetProperty(key);
            if (property != null)
            {
                var propertyValue = property.GetValue(item)?.ToString();
                if (propertyValue == value)
                {
                    result.Add(item);
                }
            }
        }

        return result;
    }

    public T? GetOneBy(string key, string value)
    {
        return GetBy(key, value).FirstOrDefault();
    }

    public T? GetById(int id)
    {
        return GetOneBy("id", id.ToString());
    }

    // Find item by ID in .csv and delete it from the file
    public bool DeleteById(int id)
    {
        var lines = GetAllRaw(true);

        var updatedLines = new List<string> { lines[0] };
        bool entityFound = false;

        foreach (var line in lines.Skip(1))
        {
            string itemId = line.Split(',').ElementAtOrDefault(0) ?? "";
            if (itemId == id.ToString()) entityFound = true;
            else updatedLines.Add(line);
        }

        if (!entityFound) return false;

        File.WriteAllLines(filePath, updatedLines);

        return true;
    }

    public T Create(string line)
    {
        // Remove any empty lines from the file before appending new data
        // So file will always have valid and clean data
        FileUtils.RemoveEmptyLines(filePath);

        // add logic to automatically increment ID

        File.AppendAllText(filePath, "\n" + line);
        return lineParser(line);
    }

    public T? Update(int id, Dictionary<string, string> updatedFields)
    {
        var lines = GetAllRaw(true);
         Console.WriteLine(lines[0]);
        // convert to lower to ignore any case differences
        var header = lines[0].ToLower().Split(',');
        Console.WriteLine(header);
        var idIndex = Array.IndexOf(header, "id");
        Console.WriteLine(idIndex);
        for (int i = 1; i < lines.Count; i++)
        {
            var values = lines[i].Split(',');
            string itemId = values[idIndex];

            if (!String.IsNullOrEmpty(itemId) && itemId == id.ToString())
            {
                foreach (var field in updatedFields)
                {
                    var fieldIndex = Array.IndexOf(header, field.Key.ToLower());
                    if (fieldIndex != -1)
                    {
                        values[fieldIndex] = field.Value;
                    }
                }

                lines[i] = string.Join(",", values);
                File.WriteAllLines(filePath, lines);
            }
        }

        return GetById(id);
    }

    // read everything from .csv file and return in ArrayList format with line parser 
    private List<T> GenericGetAll<T>(Func<string, T> parser, bool includeHeader = false)
    {
        var result = new List<T>();

        try
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);
            var linesToLoop = includeHeader ? lines : lines.Skip(1);

            foreach (var line in linesToLoop)
            {
                // Apply the parser function to each value in the line and add to the result
                result.Add(parser(line));
            }
        }
        catch (Exception ex)
        {
            // Handle any errors that occur during file reading
            Console.WriteLine($"Error reading CSV file: {ex.Message}");
        }

        return result;
    }
    
}
namespace practice1_Batko_Daniel_KN24.Modules.Shared;

public static class FileUtils
{
    public static void RemoveEmptyLines(string filePath)
    {
        try
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);

            // Check if the first line is a valid header and save it
            string header = lines[0];

            var validLines = new List<string>();

            // Filter out lines that are empty or contain only spaces
            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                validLines.Add(line);   
            }

            // If there's no valid line, we just write the header back to the file
            if (validLines.Count == 0)
            {
                File.WriteAllText(filePath, header);
                return;
            }

            // Add the header back to the list of valid lines
            validLines.Insert(0, header);

            // Overwrite the file with the valid lines
            File.WriteAllLines(filePath, validLines);

            // For debug purposes:
            // Console.WriteLine("Empty lines removed, only valid lines kept.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
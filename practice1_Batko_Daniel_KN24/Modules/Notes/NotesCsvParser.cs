using practice1_Batko_Daniel_KN24.Modules.Notes.Enitites;
using practice1_Batko_Daniel_KN24.Modules.Shared;

namespace practice1_Batko_Daniel_KN24.Modules.Notes;

 public class NotesCsvParser
 {
    public static NotesEntity NotesFromCsv(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            throw new ArgumentException("CSV line is empty or null");
        }
        
        string[] parts = line.Split(',');
        
        int.TryParse(parts.ElementAtOrDefault(0), out var id);
        
        return new NotesEntity(
             id,
            parts.ElementAtOrDefault(1) ?? Constants.DefaultValue,
            parts.ElementAtOrDefault(2) ?? Constants.DefaultValue,
            parts.ElementAtOrDefault(3) ?? Constants.DefaultValue
        );  
    }
}
            

    namespace practice1_Batko_Daniel_KN24.Modules.Notes
    {
        public class NotesCreator
        {
            private static readonly string NotesDirectory = "Data/DataBase/Notes";
            
            static NotesCreator()
            {
                if (!Directory.Exists(NotesDirectory))
                {
                    Directory.CreateDirectory(NotesDirectory);
                }
            }
            
            public static void CreateNote(string email, string title, string content, string time)
            {
                Directory.CreateDirectory(NotesDirectory);
                
                string filePath = Path.Combine(NotesDirectory, $"{email}.csv");
                
                if (!File.Exists(filePath))
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("ID,Title,Content,Time");
                    }
                }
                
                string noteId = GenerateNoteId(filePath);
                
                string noteLine = $"{noteId},\"{title}\",\"{content}\",\"{time}\"";
                
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine(noteLine);
                }

                Console.WriteLine($"The note was successfully created for the user {email}.");
            }
            
            public static bool DeleteNoteById(string noteId, string email)
            {
                string filePath = Path.Combine(NotesDirectory, $"{email}.csv");

                if (!File.Exists(filePath))
                {
                    Console.WriteLine("No notes found for this user.");
                    return false;
                }

                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length <= 1)
                {
                    Console.WriteLine("No notes available to delete.");
                    return false;
                }
                
                
                bool isDeleted = false;
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i == 0 || !lines[i].StartsWith($"{noteId},"))
                        {
                            writer.WriteLine(lines[i]);
                        }
                        else
                        {
                            isDeleted = true; 
                        }
                    }
                }

                return isDeleted;
            }
            
            public static string GetNoteByIdForUser(string noteId, string userEmail)
            {
                try
                {
                    string filePath = $"Data/DataBase/Notes/{userEmail}.csv";
                    
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"No notes file found for user: {userEmail}");
                        return null;
                    }
                    
                    var lines = File.ReadAllLines(filePath);

                    foreach (var line in lines)
                    {
                        if (line.StartsWith(noteId + ","))
                        {
                            return line;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accessing notes: {ex.Message}");
                }

                return null; 
            }


            
                private static string GenerateNoteId(string filePath)
                {
                    if (!File.Exists(filePath))
                    {
                        return "1";
                    }

                    string[] lines = File.ReadAllLines(filePath);
                    if (lines.Length <= 1)
                    {
                        return "1";
                    }

                    string lastLine = lines[^1];
                    string[] parts = lastLine.Split(',');
                    if (int.TryParse(parts[0], out int lastId))
                    {
                        return (lastId + 1).ToString();
                    }

                    throw new InvalidOperationException("Failed to read last note ID. ");
                }
            
        }
    }

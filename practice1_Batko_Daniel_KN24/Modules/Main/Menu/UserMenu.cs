using practice1_Batko_Daniel_KN24.Modules.Config;
using practice1_Batko_Daniel_KN24.Modules.Notes;
using practice1_Batko_Daniel_KN24.Modules.Shared;
using practice1_Batko_Daniel_KN24.Modules.Shared.Exceptions;
using practice1_Batko_Daniel_KN24.Modules.User.Entities;
using Spectre.Console;

namespace practice1_Batko_Daniel_KN24.Modules.Main.Menu;

public class UserMenu
{
    private readonly ConfigService _configService = new();

    public void RenderMenu(UserEntity user)
    {
        while (true)
        {
            try
            {
                Console.Clear();
                AnsiConsole.Markup($"[cyan1]Hello[/]" +
                                   $"[seagreen2] {user.FirstName}:globe_showing_europe_africa:!!![/]\n");
                Console.ResetColor();

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Please select an option:")
                        .PageSize(10)
                        .AddChoices(
                            "Create note",
                            "Edit note",
                            "Delete note",
                            "Search note",
                            "View note",
                            "Sign out",
                            "Exit program")
                );

                switch (choice)
                {
                    case "Create note":
                        CreateNote(user);
                        break;
                    case "Edit note":
                        EditNote(user);
                        break;
                    case "Delete note":
                        DeleteNote(user);
                        break;
                    case "Search note":
                        SearchNoteWithTable(user);
                        break;
                    case "View note":
                        ViewNotesWithTable(user);
                        break;
                    case "Sign out":
                        SignOut();
                        return;
                    case "Exit program":
                        Exit();
                        return;
                    default:
                        throw new InvalidMenuOption();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
    
    public void SignOut()
    {
        _configService.DeleteByKey(ConfigConstants.UserSession);
        AnsiConsole.Markup("[red]Bye bye...\n[/]");
        ConsoleUtils.AnyKey.Pause();
        App.Start();
    }

    public void Exit()
    {
        Console.Clear();
        AnsiConsole.Markup("[underline green]Exiting the application...[/]\n");
        Environment.Exit(0);
    }

    private void CreateNote(UserEntity user)
    {
        try
        {
            string email = user.Email;
            string title = ConsoleUtils.ReadUserInput("Enter a note title: ");
            while (!ValidationUtils.IsValidNotes(title))
            {
                AnsiConsole.Markup("[underline red]Invalid title! Please, try again!\n[/]");
                title = ConsoleUtils.ReadUserInput("Enter a note title: ");
            }

            string content = ConsoleUtils.ReadUserInput("Enter the content of the note: ");
            while (!ValidationUtils.IsValidNotes(content))
            {
                AnsiConsole.Markup("[underline red]Invalid content! Please, try again!\n[/]");
                content = ConsoleUtils.ReadUserInput("Enter the content of the note: ");
            }

            string time = ConsoleUtils.ReadUserInput("Enter the date and time for the note (in dd.MM.yyyy HH:mm format): ");
            while (!DateTime.TryParseExact(time, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
            {
                AnsiConsole.Markup("[underline red]Incorrect date and time format. Please use dd.MM.yyyy HH:mm format.\n[/]");
                time = ConsoleUtils.ReadUserInput("Enter the date and time for the note (in dd.MM.yyyy HH:mm format): ");
            }

            NotesCreator.CreateNote(email, title, content, time);
            Console.WriteLine("Note successfully created!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        ConsoleUtils.AnyKey.Pause();
    }

    private void DeleteNote(UserEntity user)
    {
        try
        {
            Console.Write("Enter the ID of the note to delete: ");
            string noteId = Console.ReadLine();

            AnsiConsole.Markup("[maroon]Are you sure you want to delete this note?[/] ([green]Y[/]/[red]N[/]): ");
            string confirmation = Console.ReadLine()?.Trim().ToUpper();

            if (confirmation != "Y")
            {
                Console.WriteLine("Note deletion canceled.");
                return;
            }

            bool isDeleted = NotesCreator.DeleteNoteById(noteId, user.Email);

            Console.WriteLine(isDeleted
                ? "Note successfully deleted."
                : "Note not found or could not be deleted.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        ConsoleUtils.AnyKey.Pause();
    }

    private void SearchNoteWithTable(UserEntity user)
    {
        try
        {
            Console.Write("Enter the ID of the note to search for: ");
            string noteId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(noteId))
            {
                Console.WriteLine("Note ID cannot be empty. Please try again.");
                return;
            }

            string email = user.Email;
            string note = NotesCreator.GetNoteByIdForUser(noteId, email);

            if (!string.IsNullOrEmpty(note))
            {
                RenderNoteTable(new[] { note });
            }
            else
            {
                Console.WriteLine("No note found with the specified ID.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        ConsoleUtils.AnyKey.Pause();
    }

    private void ViewNotesWithTable(UserEntity user)
    {
        try
        {
            string email = user.Email;
            string filePath = $"Data/DataBase/Notes/{email}.csv";

            if (!File.Exists(filePath))
            {
                Console.WriteLine("No notes found.");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            var lines = File.ReadAllLines(filePath);
            if (lines.Length <= 1)
            {
                Console.WriteLine("No notes available.");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            RenderNoteTable(lines.Skip(1));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        ConsoleUtils.AnyKey.Pause();
    }

    private void RenderNoteTable(IEnumerable<string> notes)
    {
        var table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Title");
        table.AddColumn("Content");
        table.AddColumn("Time");

        foreach (var note in notes)
        {
            var parts = note.Split(',');
            table.AddRow(
                Markup.Escape(parts[0]),
                Markup.Escape(parts[1].Trim('"')),
                Markup.Escape(parts[2].Trim('"')),
                Markup.Escape(parts[3].Trim('"'))
            );
        }

        AnsiConsole.Render(table);
    }


    private void EditNote(UserEntity user)
    {
        try
        {
            string email = user.Email;
            string filePath = Path.Combine("Data/DataBase/Notes", $"{email}.csv");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"No notes found for user {email}.");
                return;
            }

            Console.Write("Enter the ID of the note to edit: ");
            string noteId = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(noteId))
            {
                Console.WriteLine("Note ID cannot be empty.");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            var lines = File.ReadAllLines(filePath).ToList();
            int index = lines.FindIndex(line => line.StartsWith($"{noteId},"));
            if (index == -1)
            {
                Console.WriteLine($"No note found with ID {noteId}.");
                ConsoleUtils.AnyKey.Pause();
                return;
            }

            var parts = lines[index].Split(',');
            Console.WriteLine("\nCurrent note details:");
            Console.WriteLine($"Title: {parts[1]}");
            Console.WriteLine($"Content: {parts[2]}");
            Console.WriteLine($"Time: {parts[3]}");

            Console.Write("Enter new Title (leave blank to keep current): ");
            string newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
            {
                parts[1] = $"\"{newTitle}\"";
            }

            Console.Write("Enter new Content (leave blank to keep current): ");
            string newContent = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newContent))
            {
                parts[2] = $"\"{newContent}\"";
            }

            Console.Write("Enter new Time (leave blank to keep current, format dd.MM.yyyy HH:mm): ");
            string newTime = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTime) &&
                DateTime.TryParseExact(newTime, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out _))
            {
                parts[3] = $"\"{newTime}\"";
            }
            else if (!string.IsNullOrWhiteSpace(newTime))
            {
                Console.WriteLine("Invalid time format. Keeping the current value.");
            }

            lines[index] = string.Join(',', parts);
            File.WriteAllLines(filePath, lines);

            Console.WriteLine("Note updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        ConsoleUtils.AnyKey.Pause();
    }
}

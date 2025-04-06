    namespace practice1_Batko_Daniel_KN24.Modules.Notes.Enitites;

    public struct NotesEntity
    {
        public int ID {get; set;}
        public string Title{get; set;}
        public string Content{get; set;}
        public string Time { get; set; } // TODO: Date to Time

        public NotesEntity(int id, string title, string content, string time)
        {
            ID = id;
            Title = title;
            Content = content;
            Time = time;
        }
    }
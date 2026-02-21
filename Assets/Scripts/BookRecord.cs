using System.Collections.Generic;

public class BookRecord
{
    public string name;
    public string password;
    public string bookName;
    public string review;
    public EmotionEnum emotion;
    public bool isOngoing;
}

public class BookRecordResponse
{
    public List<BookRecord> books;
}

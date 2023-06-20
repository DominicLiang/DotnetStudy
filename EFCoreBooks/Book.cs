using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreBooks;

public class Book
{
    public int Id { get; set; }
    public string Url { get; set; }


    public virtual People People { get; set; }
}

public class People
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }

    public int BookId { get; set; }
    //public Book Book { get; set; }
}
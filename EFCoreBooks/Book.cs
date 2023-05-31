using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreBooks;

public class Book
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string AuthorName { get; set; }
    public string Price { get; set; }
    public DateTime PubDate { get; set; }
    [ForeignKey(nameof(People))]
    public long PeopleId { get; set; }
}

public class People
{
    public long Id { get; set; }
    public string Name { get; set; }
}
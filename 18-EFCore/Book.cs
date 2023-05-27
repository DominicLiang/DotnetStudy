using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore;

public class Book
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime PubTime { get; set; }
    public double Price { get; set; }
    public string AuthorName { get; set; }
    public string director { get; set; }
    public string Description { get; set; }
    public string test { get; set; }
}
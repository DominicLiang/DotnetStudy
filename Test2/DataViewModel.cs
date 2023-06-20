using System.ComponentModel.DataAnnotations;

namespace Test2;

public class DataViewModel
{
    [MinLength(1,ErrorMessage = "Id最短为1")]
    public string Id { get; set; }

    [MinLength(6, ErrorMessage = "Name最短为6")]
    public string Name { get; set; }

    [MinLength(6, ErrorMessage = "Description最短为6")]
    public string Description { get; set; }

    [MinLength(6, ErrorMessage = "Type最短为6")]
    public string Type { get; set; }
}

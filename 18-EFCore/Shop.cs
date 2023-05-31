using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore;


public class Shop
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; private set; }

    public Geo Loaction { get; set; }

    private Shop() { }

    public Shop(string name)
    {
        Name = name;
    }

    public void ChangeDescription(string description)
    {
        Description = description;
    }
}

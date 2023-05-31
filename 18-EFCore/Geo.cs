using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _18_EFCore;

[Owned]
public class Geo
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    private Geo()
    {
        
    }

    public Geo(double latitude,double longitude)
    {
        this.Latitude = latitude;
        this.Longitude = longitude;
    }
}

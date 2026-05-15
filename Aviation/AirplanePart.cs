using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class AirplanePart
{
    public string PartNumber { get; set; } = "Default part name";
    public string Description { get; set; } = "Default desciption";
    public double Price { get; set; }

    public virtual void GetPartInfo()
    {
        Console.WriteLine($"Part Number: {PartNumber}");
        Console.WriteLine($"Description: {Description}" );
        Console.WriteLine($"Price: {Price:C}");
    }
}

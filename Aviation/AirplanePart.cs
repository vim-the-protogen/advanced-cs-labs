using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal abstract class AirplanePart
{
    public string PartNumber { get; set; } = "Default part name";
    public string Description { get; set; } = "Default desciption";
    public double Price
    { 
        get => _price;
        set => _price = value > 0 ? value : throw NegativePriceException;
    }

    private static Exception NegativePriceException =>
        new ArgumentException("Price cannot be negative");

    public double _price;
}

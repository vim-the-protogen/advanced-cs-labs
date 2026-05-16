using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal abstract partial class AirplanePart
{
    public string PartNumber
    {
        get => _partNumber;
        set
        {
            if (value is null)
            {
                throw new PartNumberInvalidException($"{nameof(PartNumber)} cannot be assigned to null");
            }

            if (illegalPartNumberChars().IsMatch(value))
            {
                throw new PartNumberInvalidException($"Part number, \"{value}\", contains a space, \"*\", or \"?\"");
            }

            _partNumber = value;
        }
    }

    public string Description { get; set; } = "Default desciption";
    public double Price
    { 
        get => _price;
        set => _price = value > 0 ? value : throw _negativePriceException;
    }

    private static readonly Exception _negativePriceException =
        new ArgumentException("Price cannot be negative");

    public double _price;
    private string _partNumber = "Default part name";

    [GeneratedRegex(@"[*?\s]|.{,0}")]
    private static partial Regex illegalPartNumberChars();
}

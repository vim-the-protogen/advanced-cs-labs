using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EnginePart: AirplanePart
{
    public string EngineType { get; set; } = "Default engine type";

    public override void GetPartInfo()
    {
        base.GetPartInfo();
        Console.WriteLine($"Engine Type: {EngineType}");
    }
}

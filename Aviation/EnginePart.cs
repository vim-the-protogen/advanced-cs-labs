using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EnginePart: AirplanePart, ISelfTest
{
    public string EngineType { get; set; } = "Default engine type";

    public int SelfTest()
    {
        Console.WriteLine("Engine self testing...");
        return 1;
    }
}

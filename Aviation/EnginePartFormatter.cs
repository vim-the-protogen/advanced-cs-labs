using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EnginePartFormatter
{
    public static string GetPartInfo(EnginePart ep) =>
        $"{AirplanePartFormatter.GetPartInfo(ep)},\n" +
        $"Engine Type {ep.EngineType}";
}

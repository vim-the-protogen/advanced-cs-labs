using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal static class PartFormatter
{
    private static string getBasePartInfo(this AirplanePart ap) =>
        $"Part Number: {ap.PartNumber}\n" +
        $"Description: {ap.Description}\n" +
        $"Price: {ap.Price:C}";

    public static string GetPartInfo(this AirplanePart ap) =>
        ap switch
        {
            EnginePart ep => $"{ap.getBasePartInfo()},\n" +
                             $"Engine Type: {ep.EngineType}",
            _ => getBasePartInfo(ap),
        };
}

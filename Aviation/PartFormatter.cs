using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal static class PartFormatter
{
    private static string getBasePartInfo(this AirplanePart ap,
                                          string prefix = "") =>
        $"{prefix}Part Number: {ap.PartNumber}\n" +
        $"{prefix}Description: {ap.Description}\n" +
        $"{prefix}Price: {ap.Price:C}";

    public static string GetPartInfo(this AirplanePart ap, string prefix = "") =>
        ap switch
        {
            EnginePart ep => $"{ap.getBasePartInfo(prefix)},\n" +
                             $"{prefix}Engine Type: {ep.EngineType}",
            _ => ap.getBasePartInfo(prefix),
        };
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class AirplanePartFormatter
{
    public static string GetPartInfo(AirplanePart ap) =>
        $"Part Number: {ap.PartNumber}\n" +
        $"Description: {ap.Description}\n" +
        $"Price: {ap.Price:C}";
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EngineFactory
{
    public Dictionary<string, AirplanePart> Cache
    {
        get;
        private set;
    } = [];

    /// <summary>
    /// Load all <see cref="EnginePart"/>s in the CSV at
    /// <paramref name="path"/>
    /// </summary>
    /// <param name="path">The location of the csv</param>
    /// <returns>
    /// All engine parts that are defined in the CSV at
    /// <paramref name="path"/>
    /// </returns>
    /// <exception cref="FileFormatException"></exception>
    public IEnumerable<AirplanePart> LoadEngineParts(string path)
    {
        IEnumerable<string[]> csvLines =
            File.ReadLines(path)
                .Select(SplitAndSanitizeLine)
                .Select(VerifyColumnCount);

        bool headersExist =
            csvLines.First() is ["PartNumber", "Description", "Price", "EngineType"];

        if (!headersExist)
        {
            throw new FileFormatException($"Headers in {path} are malformed");
        }

        EnginePart part;
        foreach (var line in csvLines.Skip(1))
        {
            part = new EnginePart()
            {
                PartNumber = line[0],
                Description = line[1],
                Price = double.Parse(line[2]),
                EngineType = line[3],
            };

            Cache.Add(part.PartNumber, part);
            yield return part;
        }
    }

    private static string[] SplitAndSanitizeLine(string line) =>
        [.. line.Split(',').Select(s => s.Trim())];

    private static string[] VerifyColumnCount(string[] line) =>
        line.Length == 4
        ? line
        : throw new FormatException($"Number of items is not equal to " +
                                    $"four: {line}");
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace com.ntier.Aviation;

internal class EngineFactory(EngineInventoryManager manager) : IEnumerable<AirplanePart>
{
    private readonly EngineInventoryManager inventoryManager = manager;

    public AirplanePart? this[string partNumber] =>
        inventoryManager[partNumber];

    public event InventoryEventHandler? InventoryExhausted
    {
        add => inventoryManager.InventoryExhausted += value;
        remove => inventoryManager.InventoryExhausted -= value;
    }

    public AirplanePart? Release(string partNumber) =>
        inventoryManager.Release(partNumber);

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
            csvLines.First() is ["PartNumber", "Description", "Price", "EngineType", "Count", "Threshold"];

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
                Count = int.Parse(line[4]),
                Threshold = int.Parse(line[5]),
            };

            inventoryManager.Add(part);
            yield return part;
        }
    }

    private static string[] SplitAndSanitizeLine(string line) =>
        [.. line.Split(',').Select(s => s.Trim())];

    private static string[] VerifyColumnCount(string[] line) =>
        line.Length == 6
        ? line
        : throw new FormatException($"Number of items is not equal to " +
                                    $"four: {line}");

    public IEnumerator<AirplanePart> GetEnumerator() =>
        inventoryManager.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

namespace com.ntier.Aviation;

internal static class EngineExtensions
{
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
    public static async IAsyncEnumerable<AirplanePart> LoadEngineParts(this string path)
    {
        static AirplanePart ctr(string[] line)
        {
            // Simulate a long construction time
            Thread.Sleep(100);
            return new EnginePart()
            {
                PartNumber = line[0],
                Description = line[1],
                Price = double.Parse(line[2]),
                EngineType = line[3],
                Count = int.Parse(line[4]),
                Threshold = int.Parse(line[5]),
            };
        }

        IAsyncEnumerable<string[]> csvLines = File.ReadLinesAsync(path)
            .Select(SplitAndSanitizeLine);
        string[] headers = await csvLines.FirstAsync();

        bool headersExist =
            headers is ["PartNumber", "Description", "Price", "EngineType", "Count", "Threshold"];

        if (!headersExist)
        {
            throw new FileFormatException($"Headers in {path} are malformed");
        }

        var parts = csvLines.Skip(1)
                            .Select(VerifyColumnCount)
                            .Select(ctr);

        await foreach (var p in parts)
        {
            yield return p;
        }
    }

    private static string[] SplitAndSanitizeLine(string line)
        => [.. line.Split(',').Select(s => s.Trim())];

    private static string[] VerifyColumnCount(string[] line)
        => line.Length == 6
           ? line
           : throw new FormatException($"Number of items is not equal to " +
                                       $"four: {line}");
}

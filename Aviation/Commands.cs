namespace com.ntier.Aviation;

internal static class Commands
{
    private const string _helpMessage =
    $"""
    help                                                    -  displays this message
    list [price [ascending|descending|between <min> <max>]  -  display all parts. Optionally
         [EngineType <Engine type>]                            sorted by price, ascending or
                                                               descending, or can be filtered
                                                               by price range or engine type
    get <Part Number>                                       -  display the part that has <Part Number>
    release <Part Number>                                   -  decrement the part count, and display or
                                                               notify that the part is exausted
    exit                                                    -  exit the program
    """;

    /// <summary>
    /// Parse the command that is represented by <paramref name="args"/>
    /// </summary>
    /// <param name="args">
    /// The input command
    /// </param>
    /// <returns>
    /// An asynchronous function that returns a bool depending on whether the
    /// program should exit, e.g. the "exit" command
    /// </returns>
    public static Func<Task<EngineManager>, Task<bool>> ParseCommand(this string[] args)
        => args switch
        {
            ["exit"] => _ => Task.FromResult("").Write(false),
            ["get", _] => args.ConstructCommand(GetCommand, true),
            ["list", ..] => args.ListCommands(),
            ["release", _] => args.ConstructCommand(ReleaseCommand, true),
            ["help"] => _ => Task.FromResult(_helpMessage).Write(true),
            [""] => _ => Task.FromResult(string.Empty).Write(true),
            _ => _ => args.InvalidCommand().Write(true),
        };

    public static Func<Task<EngineManager>, Task<bool>> ListCommands(this string[] args)
        => args switch
        {
            [_] => args.ConstructCommand(ListCommand, true),
            [_, "EngineType", _]
                => args.ConstructCommand(ListByEngineType, true),
            [_, "price", "between", _, _]
                => args.ConstructCommand(ListByPriceBetween, true),
            [_, "price", "ascending"] 
                => args.ConstructCommand(ListByPriceAscendingCommand, true),
            [_, "price", "descending"]
                => args.ConstructCommand(ListByPriceDescendingCommand, true),
            _ => _ => args.InvalidCommand().Write(true),
        };

    private static Func<Task<EngineManager>, Task<bool>>
        ConstructCommand(this string[] args,
                         Func<Task<EngineManager>, string[], Task<string>> cmd,
                         bool result)
        => factory => cmd(factory, args).Write(result);

    private static async Task<bool> Write(this Task<string> output, bool result)
        => await (output.Result).Write(result);

    private static Task<bool> Write(this string output, bool result)
    {
        Console.WriteLine($"{output}");
        return Task.FromResult(result);
    }

    private static bool EngineTypeMatches(this string engineType,
                                          EnginePart? part)
        => part is not null &&
           part.EngineType.Equals(engineType,
                                  StringComparison.CurrentCultureIgnoreCase);
    private static async Task<string> ListByEngineType(Task<EngineManager> parts,
                                                       string[] args)
        => (await parts).Where(p => p is EnginePart)
                        .Select(p => p as EnginePart)
                        .Where(args[2].EngineTypeMatches)
                        .Select(p => p.GetPartInfo("\t"))
                        .Prepend($"Parts with Engine Type, {args[2]}:")
                        .Print();

    private static async Task<string> ListByPriceBetween(Task<EngineManager> parts,
                                                         string[] args)
    {
        if (!double.TryParse(args[3], out double min))
        {
            return $"Failed to Parse {args[3]}";
        }

        if (!double.TryParse(args[4], out double max))
        {
            return $"Failed to Parse {args[4]}";
        }

        if (min > max)
        {
            return $"Error: min, {min}, is greater than max, {max}";
        }

        return (await parts).Where(p => p.Price >= min && p.Price <= max)
                            .Select(p => p.GetPartInfo("\t"))
                            .Print();
    }


    private static Task<string> InvalidCommand(this string[] args)
        => Task.FromResult($"Invalid Command: {string.Join(' ', args)}");

    private static async Task<string> ReleaseCommand(Task<EngineManager> parts,
                                                     string[] args)
        => (await parts).Release(args[1]) is AirplanePart part
           ? part.GetPartInfo("\t")
           : $"The part, {args[1]}, is exhausted";

    private static async Task<string> GetCommand(Task<EngineManager> parts,
                                                 string[] args)
        => (await parts).FirstOrDefault(p => p.PartNumber == args[1]) is AirplanePart part
           ? part.GetPartInfo("\t")
           : $"Error: Could not find part, {args[1]}";

    private static async Task<string>
        ListByPriceAscendingCommand(Task<EngineManager> parts,
                                    string[] args)
            => (await parts).OrderBy(ap => ap.Price)
                            .Select(ap => ap.GetPartInfo("\t"))
                            .Prepend("Parts by price, ascending:")
                            .Print();

    private static async Task<string>
        ListByPriceDescendingCommand(Task<EngineManager> parts,
                                     string[] _)
        => (await parts).OrderBy(ap => ap.Price)
                        .Reverse()
                        .Select(ap => ap.GetPartInfo("\t"))
                        .Prepend("Parts by price, ascending:")
                        .Print();

    /// <summary>
    /// Generate a <see cref="string"/> containing all
    /// <see cref="AirplanePart"/>s in <paramref name="parts"/>
    /// </summary>
    /// <param name="parts">Storer of the database</param>
    /// <returns></returns>
    private static async Task<string> ListCommand(Task<EngineManager> parts,
                                                  string[] _)
        => (await parts).Select(p => p.GetPartInfo("\t"))
                        .Prepend("Parts by part number, ascending:")
                        .Print();

    private static string Print(this IEnumerable<string> list)
        => $"{(list.Any()
               ? list.Aggregate((a, b) => $"{a}\n{b}\n")
               : "No Engines Found")}";
}
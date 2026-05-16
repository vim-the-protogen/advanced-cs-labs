using System.Runtime.CompilerServices;

//using Command = System.Func<com.ntier.Aviation.EngineFactory, bool>;
namespace com.ntier.Aviation;

internal static class Commands
{
    private const string _helpMessage =
    $"""
    help                                                   -  displays this message
    list [price [ascending|descending|between <min> max>]  -  display all parts. Optionally
         [EngineType <Engine type>]                           sorted by price, ascending or
                                                              descending, or can be filtered
                                                              by price range or engine type
    get <Part Number>                                      -  display the part that has <Part Number>
    release <Part Number>                                  -  decrement the part count, and display or
                                                              notify that the part is exausted
    exit                                                   -  exit the program
    """;

    public static Func<EngineFactory, bool> Execute(this string[] args) =>
        args switch
        {
            ["exit"] => args.ConstructCommand(ExitCommand, false),
            ["get", _] => args.ConstructCommand(GetCommand, true),
            ["list", ..] => args.ListCommands(),
            ["release", _] => args.ConstructCommand(ReleaseCommand, true),
            ["help"] => _ => _helpMessage.Write(true),
            [""] => _ => string.Empty.Write(true),
            _ => args.InvalidCommand(),
        };

    public static Func<EngineFactory, bool> ListCommands(this string[] args)
        => args switch
        {
            [_] => args.ConstructCommand(ListCommand, true),
            [_, "EngineType", _] => args.ConstructCommand(ListByEngineType, true),
            [_, "price", "between", _, _] => args.ConstructCommand(ListByPriceBetween, true),
            [_, "price", "ascending"] => args.ConstructCommand(ListByPriceAscendingCommand, true),
            [_, "price", "descending"] => args.ConstructCommand(ListByPriceDescendingCommand, true),
            _ => args.InvalidCommand()
        };

    private static bool EngineTypeMatches(this string engineType,
                                          EnginePart? part)
        => part is not null &&
           part.EngineType.Equals(engineType,
                                  StringComparison.CurrentCultureIgnoreCase);
    private static string ListByEngineType(this EngineFactory parts,
                                           string[] args)
        => parts.Where(p => p is EnginePart)
                .Select(p => p as EnginePart)
                .Where(args[2].EngineTypeMatches)
                .Select(p => p.GetPartInfo("\t"))
                .Prepend($"Parts with Engine Type, {args[2]}:")
                .Print();

    private static string ListByPriceBetween(this EngineFactory parts,
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

        return parts.Where(p => p.Price >= min && p.Price <= max)
                    .Select(p => p.GetPartInfo("\t"))
                    .Print();
    }

    private static Func<EngineFactory, bool> InvalidCommand(this string[] args)
        => _ => $"Invalid Command: {string.Join(' ', args)}".Write(true);

    private static Func<EngineFactory, bool>
        ConstructCommand(this string[] args,
                        Func<EngineFactory, string[], string> cmd,
                        bool result)
        => factory => cmd(factory, args).Write(result);

    private static bool Write(this string output, bool result)
    {
        Console.WriteLine($"{output}");
        return result;
    }

    private static string ReleaseCommand(this EngineFactory parts,
                                         string[] args)
        => parts.Release(args[1]) is AirplanePart part
           ? part.GetPartInfo("\t")
           : $"The part, {args[1]}, is exhausted";


    private static string ExitCommand(this IEnumerable<AirplanePart> parts,
                                      string[] args)
        => "Program Complete";

    private static string GetCommand(this IEnumerable<AirplanePart> parts,
                                     string[] args)
        => parts.FirstOrDefault(p => p.PartNumber == args[1]) is AirplanePart part
           ? part.GetPartInfo("\t")
           : $"Error: Could not find part, {args[1]}";

    private static string
        ListByPriceAscendingCommand(this IEnumerable<AirplanePart> parts,
                                    string[] args)
            => parts.OrderBy(ap => ap.Price)
                    .Select(ap => ap.GetPartInfo("\t"))
                    .Prepend("Parts by price, ascending:")
                    .Print();

    private static string
        ListByPriceDescendingCommand(this IEnumerable<AirplanePart> parts,
                                     string[] _)
        => parts.OrderBy(ap => ap.Price)
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
    private static string ListCommand(this IEnumerable<AirplanePart> parts,
                                      string[] _)
        => parts.Select(p => p.GetPartInfo("\t"))
                .Prepend("Parts by part number, ascending:")
                .Print();

    private static string Print(this IEnumerable<string> list)
        => $"{(list.Any()
               ? list.Aggregate((a, b) => $"{a}\n{b}\n")
               : "No Engines Found")}";
}
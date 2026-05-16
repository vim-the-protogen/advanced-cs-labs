using System.Runtime.CompilerServices;

//using Command = System.Func<com.ntier.Aviation.EngineFactory, bool>;
namespace com.ntier.Aviation;

internal static class Commands
{
    public static Func<EngineFactory, bool> Execute(this string[] args) =>
        args switch
        {
            ["exit"] => args.ConstructCommand(ExitCommand, false),
            ["list"] => args.ConstructCommand(ListCommand, true),
            ["get", _] => args.ConstructCommand(GetCommand, true),
            ["list", "price", "ascending"] => 
                args.ConstructCommand(ListByPriceAscendingCommand, true),
            ["list", "price", "descending"] =>
                args.ConstructCommand(ListByPriceDescendingCommand, true),
            ["release", _] => args.ConstructCommand(ReleaseCommand, true),
            ["help"] => _ => 
                $"""
                help                                     -  displays this message
                list [price ascending|price descending]  -  display all parts. Optionally
                                                            sorted by price, ascending 
                                                            or descending
                get <Part Number>                        -  display the part that has <Part Number>
                release <Part Number>                    -  decrement the part count, and display or
                                                            notify that the part is exausted
                exit                                     -  exit the program
                """.Write(true),
            [""] => _ => string.Empty.Write(true),
            _ => _ => $"Invalid Command: {string.Join(' ', args)}".Write(true),
        };

    private static Func<EngineFactory, bool>
        ConstructCommand(this string[] args,
                        Func<EngineFactory, string[], string> cmd,
                        bool result)
        => factory => cmd(factory, args).Write(result);

    private static bool Write(this string output, bool result)
    {
        Console.WriteLine($"{output}\n");
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
           : $"\nCould not find part: {args[1]}\n";

    private static string
        ListByPriceAscendingCommand(this IEnumerable<AirplanePart> parts,
                                    string[] args)
            => parts.OrderBy(ap => ap.Price)
                    .Select(ap => ap.GetPartInfo("\t"))
                    .Prepend("Parts by price, ascending:")
                    .Print();

    private static string
        ListByPriceDescendingCommand(this IEnumerable<AirplanePart> parts,
                                     string[] args)
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
    /// <param name="argss"></param>
    /// <returns></returns>
    private static string ListCommand(this IEnumerable<AirplanePart> parts,
                                      string[] _)
        => parts.Select(p => p.GetPartInfo("\t"))
                .Prepend("Parts by part number, ascending:")
                .Print();

    private static string Print(this IEnumerable<string> list)
        => $"{list.Aggregate((a, b) => $"{a}\n{b}\n")}";

}
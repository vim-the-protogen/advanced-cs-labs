using com.ntier.Aviation;
using System;
using System.Collections.Immutable;

namespace com.ntier.Aviation;

internal class Program
{
    static void Main(string[] args)
    {
        Program self = new();
        string path = @"C:\Users\h6\source\repos\Lab 4.2\Aviation\Resources\parts.csv";
        EngineFactory factory = new();
        List<AirplanePart> parts = [];

        try
        {
            parts = [.. factory.LoadEngineParts(path)];
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine($"File not found: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"File error: {ex.Message}");
        }
        catch (FormatException ex)
        {
            Console.WriteLine($"Format error: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (FileFormatException ex) {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (PartNumberInvalidFormatException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        bool runloop = true;
        while (runloop)
        {
            runloop = self.Execute(factory);
        }
    }

    private bool Execute(EngineFactory factory)
    {
        // I want to figure out a way to make the pattern matching better
        // without needing to use a dictionary.
        // Maybe use an enum? I don't think printing all the commands is
        // strictly necessary. Should I create a a help command?
        const string defaultPrompt =
            $"""
            Here are the available commands:
                exit
                list
                get <Part Number>
                listbypriceascending
                listbypricedescending

            Cmd: 
            """;
        
        Console.Write(defaultPrompt);
        string[] input = (Console.ReadLine() ?? "").Split(' ');
        var cmd = Command(input);

        return cmd(factory);
    }

    private Func<EngineFactory, bool> Command(string[] input) =>
        input switch
        {
            ["exit"] => f => ExitCommand(input, f),
            ["list"] => f => ListCommand(input, f),
            ["get", _] => f => GetCommand(input, f),
            ["listbypriceascending"] => f => ListByPriceAscendingCommand(input, f),
            ["listbypricedescending"] => f => ListByPriceDescendingCommand(input, f),
            [""] => _ =>
            {
                Console.WriteLine("");
                return true;
            },
            _ => _ =>
            {
                Console.WriteLine($"Invalid Command: {input[0]}");
                return true;
            }
        };

    private bool ListByPriceDescendingCommand(string[] args, EngineFactory factory)
    {
        var engines = factory.Cache
                             .Select(kvp => kvp.Value)
                             .OrderBy(ap => ap.Price)
                             .Reverse()
                             .Select(ap => ap.GetPartInfo());
        Print(engines);
        return true;
    }
    private bool ListByPriceAscendingCommand(string[] args, EngineFactory factory)
    {
        var engines = factory.Cache
                             .Select(kvp =>  kvp.Value)
                             .OrderBy(ap => ap.Price)
                             .Select(ap => ap.GetPartInfo());
        Print(engines);
        return true;
    }
    private bool ExitCommand(string[] args, EngineFactory factory)
    {
        Console.WriteLine("Program Complete");
        return false;
    }

    private bool ListCommand(string[] args, EngineFactory factory)
    {
        Print(factory.Cache.Select(kvp => kvp.Value.GetPartInfo()));
        return true;
    }

    private bool GetCommand(string[] args, EngineFactory factory)
    {
        string partNumber = args[1];
        Console.WriteLine();
        if (factory.Cache.TryGetValue(partNumber, out var part))
        {
            Console.WriteLine(part.GetPartInfo());
        }
        else
        {
            Console.WriteLine($"Could not find part: {partNumber}\n");
        }
        Console.WriteLine();
        return true;
    }

    private void Print(IEnumerable<string> list)
    {
        Console.WriteLine();
        foreach (string item in list)
        {
            Console.WriteLine(item);
            Console.WriteLine();
        }
    }
}

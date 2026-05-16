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
        List<string> commands = [
            "exit",
            "list",
            "get"
        ];

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

        string printableCommands = commands.Aggregate((a, b) => $"{a}, {b}");
        string defaultPrompt =
            $"""
            Here are the available commands:
                {printableCommands}
            """;
        bool runloop = true;
        string? input = self.PromptUser(defaultPrompt);

        while (runloop)
        {
            Func<string?> command = input switch
            {
                null => () => self.PromptUser($"Invalid Commmand\n{defaultPrompt}"),
                var s when s == commands[0] => () =>
                {
                    runloop = false;
                    Console.WriteLine("Program Complete");
                    return "";
                },
                var s when s == commands[1] => () =>
                {
                    self.Print(factory.Cache.Select(kvp => kvp.Value.GetPartInfo()));
                    return self.PromptUser(defaultPrompt);
                },
                var s when s.Contains(commands[2]) => () =>
                {
                    string partNumber = s.Split(' ')[1];
                    if (factory.Cache.TryGetValue(partNumber, out var part))
                    {
                        Console.WriteLine(part.GetPartInfo());
                        return self.PromptUser(defaultPrompt);
                    }
                    else
                    {
                        return self.PromptUser($"Could not find part: {partNumber}\n" +
                                               $"{defaultPrompt}");
                    }
                    
                },
                _ => () => self.PromptUser($"Invalid Commmand\n{defaultPrompt}")
            };

            input = command();
        }
    }

    private void Print(IEnumerable<string> list)
    {
        Console.WriteLine();
        foreach (string item in list)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine("-----");
    }

    private string? PromptUser(string prompt)
    {
        Console.WriteLine($"\n{prompt}");
        return Console.ReadLine();
    }

    private static void TestException<T>(T? testParam,
                                         Func<T> selector,
                                         Action<T?> mutator)
    {
        T foo = selector();
        try
        {
            mutator(testParam);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        mutator(foo);
    }
}

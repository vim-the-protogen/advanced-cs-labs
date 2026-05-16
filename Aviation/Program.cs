using com.ntier.Aviation;
using System;
using System.Collections.Immutable;

namespace com.ntier.Aviation;

internal class Program
{
    static void Main(string[] args)
    {
        Program self = new();
        string path = @"../../../Resources/parts.csv";
        EngineFactory factory = new([]);
        List<AirplanePart> parts = [];
        factory.InventoryExhausted +=
           (_, a) => Console.WriteLine($"{a.PartNumber} " +
                                       $"is almost exausted\n");

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

        bool runloop;
        do
        {
            runloop = self.Execute(factory);
        }
        while (runloop);
    }

    private bool Execute(EngineFactory factory)
    {
        const string defaultPrompt = "Cmd: ";

        ConsoleColor temp = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(defaultPrompt);
        Console.ForegroundColor = temp;

        string[] input = (Console.ReadLine() ?? "").Split(' ');
        var cmd = input.Execute();
        

        return cmd(factory);
    }
}

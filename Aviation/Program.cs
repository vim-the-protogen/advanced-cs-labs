using com.ntier.Aviation;
using System;
using System.Collections.Immutable;

namespace com.ntier.Aviation;

internal class Program
{
    static async Task Main(string[] args)
    {
        Program self = new();
        string path = @"../../../Resources/parts.csv";

        var factoryTask = EngineManager.New(path, OnInventoryExhausted);

        bool runloop;
        do
        {
            runloop = await Execute(factoryTask);
        }
        while (runloop);

        Console.WriteLine("Program exited");
    }

    private static async Task<bool> Execute(Task<EngineManager> factoryTask)
    {
        const string defaultPrompt = "Cmd: ";

        ConsoleColor temp = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write(defaultPrompt);
        Console.ForegroundColor = temp;

        string[] input = (Console.ReadLine() ?? "").Split(' ');
        var cmd = input.ParseCommand();

        if (!factoryTask.IsCompleted)
        {
            temp = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Parts loading...");
            Console.ForegroundColor = temp;
        }

        return await TryTask(cmd(factoryTask));
    }

    private static void OnInventoryExhausted(object _, InventoryEventArgs part)
        => Console.WriteLine($"{part.PartNumber} is almost exausted\n");

    private static async Task<T> TryTask<T>(Task<T> exceptionThrowable)
    {
        try
        {
            return await exceptionThrowable;
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

        throw new Exception("Cannot procede due to unhandled exception");
    }
}

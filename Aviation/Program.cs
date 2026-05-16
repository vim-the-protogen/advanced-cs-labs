using com.ntier.Aviation;
using System;
using System.Collections.Immutable;

namespace com.ntier.Aviation;

internal class Program
{
    static void Main(string[] args)
    {
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
        finally
        {
            var keys = factory.Cache
                              .Keys
                              .OrderBy(s => s)
                              .Reverse();

            foreach ((string key, int index) in keys.Select((k, i) => (k, i)))
            {
                Console.WriteLine($"-----Part {index + 1}-----");
                Console.WriteLine($"{factory.Cache[key].GetPartInfo()}\n");
            }

            //var reverseOrderedParts = factory.Cache
            //                     .OrderBy(kvp => kvp.Value)
            //                     .Reverse()
            //                     .Select(kvp => kvp.Value);

            //foreach ((var part, int index) in reverseOrderedParts.Select((p, i) => (p, i)))
            //{
            //    Console.WriteLine($"-----Part {index + 1}-----");
            //    Console.WriteLine($"{part.GetPartInfo()}\n");
            //}

            Console.WriteLine("Program completed.");
        }
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

using com.ntier.Aviation;

namespace com.ntier.Aviation;

internal class Program
{
    static void Main(string[] args)
    {
        string path = @"C:\Users\h6\source\repos\Lab 4.2\Aviation\Resources\parts.csv";
        List<AirplanePart> parts = [];
        try
        {
            parts = [.. EngineFactory.LoadEngineParts(path)];
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
            parts.Sort();
            foreach ((var engine, int index) in parts.Select((e, i) => (e, i)))
            {
                Console.WriteLine($"-----Part {index + 1}-----");
                Console.WriteLine($"{engine.GetPartInfo()}\n");
            }

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

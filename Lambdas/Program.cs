// See https://aka.ms/new-console-template for more information
#region DON'T LOOK AT ME
B Apply<A, B>(A x, Func<A, B> f) => f(x);

void printCollection<T>(IEnumerable<T> array)
{
    Console.WriteLine("[");
    foreach (var item in array)
    {
        Console.WriteLine($"\t{item},");
    }
    Console.WriteLine("]");
}

#endregion DON'T LOOK AT ME

#region Basics
// Function variable (bound function)
Func<int, int> increment = x => x + 1;

Console.WriteLine(increment(0));
Console.WriteLine(Apply(32, increment));

increment = x => x + 27;
Console.WriteLine(increment(0));
Console.WriteLine(Apply(32, x => x * x)); // <- Anonymous Function
#endregion Basics

#region Why?
// Why lambdas?
//
// They make things easier to read!
IEnumerable<string> names = ["Alice", "Bob", "Carol", "David", "Eric"];

// Compare:
List<string> filteredNames = [];
foreach (var name in names)
{
    if (name.Length < 5)
    {
        filteredNames.Add(name);
    }
}
printCollection(filteredNames);


// To:
var foo = names.Where(name => name.Length < 5);
printCollection(foo);
#endregion Why?

#region Alternatives to GoF design patterns
// Dependancy injection is made easy with higher order functions!
void WriteToFile(string stuff)
{
    using StreamWriter file = File.AppendText("yell.log");
    file.Write(stuff);
}

var consoleYeller = new Yeller(Console.WriteLine);
var fileYeller = new Yeller(WriteToFile);

consoleYeller.Yell("I am yelling from the console!");
fileYeller.Yell("I am yelling from a file!");

// An alternative to the factory objects
// this one is hotly contested, and your mileage
// may vary
//
// This example is a toy to show off factory functions
// but it means we don't need any additional classes.
// This can mean less code, fewer files, and the abstraction
// is more local so that your mental stack has an a better
// chance of being only 5 things +/-2
IYeller Construct(Func<Action<string>, IYeller> ctr, Action<string> yellCallback) => ctr(yellCallback);

IYeller oldYeller = Construct(Yeller.New, Console.WriteLine);
oldYeller.Yell("I was made without a factory!");
#endregion

#region Delegates
// Delegates are a way are to functions what classes are to objects
// They allow for creating a named type for a function
YellerConstuctor ctr;

ctr = Yeller.New;
ctr = cb => new Yeller(cb);

IYeller Construct2(YellerConstuctor ctr, Action<string> yellCallback) => ctr(yellCallback);

oldYeller = Construct2(ctr, Console.WriteLine);
oldYeller.Yell("I was made without delegates!");

#endregion Delegates

#region Types
delegate IYeller YellerConstuctor(Action<string> yellCallback);
delegate void SomeDel();

class Yeller(Action<string> yellCallback): IYeller
{
    public void Yell(string thingToYell) => yellCallback(thingToYell);
    public static IYeller New(Action<string> yellCallback) =>
        new Yeller(yellCallback);
}

interface IYeller
{
    public void Yell(string thingToYell);
}
#endregion Types
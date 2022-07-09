// See https://aka.ms/new-console-template for more information

using PoemGenerator.BusinessLogic;

string? filePath;
Console.WriteLine(value: @"Enter a rule file path in txt format (Eg: C:\Code\rules.txt): ");
filePath = Console.ReadLine();
if (!string.IsNullOrEmpty(filePath))
{
    string[] poemRules = File.ReadAllLines(filePath);
    if (poemRules.Length > 0)
    {
        IRuleParsingEngine engine = new RuleParsingEngine();
        engine.PrepareCollections(poemRules);
        string poem = engine.GetPoem();
        Console.WriteLine(value: "------------------------------------------------------------------------------");
        Console.WriteLine(value: poem);
        Console.Write(value: "------------------------------------------------------------------------------");
        Console.ReadLine();
    }
}
else
{
    Console.WriteLine(value: "Not a valid path");
}
using TmpParser;

foreach (var line in 
         TagParser.EnumerateLines(
             TagParser.EnumerateElements(
                 TagParser.EnumerateTokens("Hello, <b>world</b>!"))))
{
    foreach (var element in line)
        Console.WriteLine(element);
    Console.WriteLine("===== END OF LINE =====");
}
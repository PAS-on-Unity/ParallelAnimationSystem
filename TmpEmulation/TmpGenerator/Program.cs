using CommandLine;
using TmpGenerator;

Parser.Default
    .ParseArguments<Options>(args)
    .WithParsed(App.Run);
    
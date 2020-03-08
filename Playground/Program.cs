using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace UserStudyTest2
{
class Program
{
    static void Main(string[] args)
    {
        var command = new RootCommand("Application to adjust volume")
        {
            new Argument<int>("new-volume")
        };
        command.Handler = CommandHandler.Create<int>(AdjustVolume);
        command.Invoke(args);
    }

    private static void AdjustVolume(int newVolume)
        => Console.WriteLine($"New volume: {newVolume}!");
}
}

# Welcome to StarFruit, aka System.CommandLine General CLI Model Prototype

This project provide a modeling layer on top of [System.CommandLine](https://github.com/dotnet/command-line-api) so you don't have to worry about the System.CommandLine API. You'll need a little code in your startup for now (we'll work on reducing this). For a tool named ManageGlobalJson your entry point might look like:

```c#
     public static int Main2(string[] args)
        {
            Strategy strategy = new Strategy("Full").SetReflectionRules();
            return strategy.Invoke((Func<ManageGlobalJson, int>)(args
                => (args switch
                    {
                         // Do your work here
                        ManageGlobalJson.Update update => update.Invoke(),
                        //... more work
                        ManageGlobalJson entry => Error("You must use a subcommand"),
                        _ => throw new InvalidOperationException("Unexpected args type")
                    })), 
                    args);
        }

```

You can define the data you want via the classes that will contain the result. This class has an argument and an option:

```c#
   public class ManageGlobalJson
   {
        public DirectoryInfo StartPathArg { get; set; }

        [Aliases("v")]
        public VerbosityLevel Verbosity { get; set; }
   }
```

You can define subcommands with derived classes. These derived classes have access to any data defined in the base class, which means your subcommand result has access to any of it's parent command's results. It's convenient for these classes to also be nested to avoid naming collisions and make everything pretty:

```c#
    public class ManageGlobalJson
    {
        public DirectoryInfo StartPathArg { get; set; }

        [Aliases("v")]
        public VerbosityLevel Verbosity { get; set; }

        public class List : ManageGlobalJson
        {
            [Aliases("o")]
            public FileInfo Output { get; set; }
        }

```

For this example, I elected to store the descriptions used for help elsewhere to keep things clear, but from classes like this you can get all the goodness of System.CommandLine. 

See the wiki for more about how you get to make up the rules used to build the System.CommandLine semantic tree from your classes. 

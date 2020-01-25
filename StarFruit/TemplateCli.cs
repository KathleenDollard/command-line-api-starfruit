using System.CommandLine.StarFruit;

namespace StarFruit.CLI
{
    public class TemplateCli
    {
    }

    public class Run : TemplateCli
    {
        [CmdArgument]
        [CmdDescription("The name of the template to run")]
        public string TemplateName { get; set; }

        public string Output { get; set; }
        public string Name { get; set; }
        public bool Force { get; set; }
        public string Language { get; set; }
        public bool DryRun { get; set; }
    }

    public class Install : TemplateCli
    {
        [CmdArgument]
        // Do we have Package support - for alternate types?
        public string Package { get; set; }
        // Explore whether this should be a different type
        public string Source { get; set; }
    }

    public class Uninstall : TemplateCli
    {
        string Package { get; set; }
    }

    public class List : TemplateCli
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class Search : TemplateCli
    {
        public Search()
        {
        }

        public string Name { get; set; }
        public string Source { get; set; }
    }

    public class Update : TemplateCli
    {
        public string Name { get; set; }
        public bool All { get; set; }
        public string Source { get; set; }
    }
}

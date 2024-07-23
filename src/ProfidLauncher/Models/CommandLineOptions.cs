using CommandLine;

namespace ProfidLauncher.Models
{
    public class CommandLineOptions
    {
        [Option('p', "port", Required = false, HelpText = "Profid port")]
        public int Port { get; set; }

        [Option('m', "opmode", Required = false, HelpText = "Profid operation mode")]
        public string OperationMode { get; set; } = "ATRIUMP";

        [Option('s', "secure", Required = false, HelpText = "Use https")]
        public bool UseSecure { get; set; }
    }
}

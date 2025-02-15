using CommandLine;

namespace MusicLib.Main;

public class CommandOptions
{
    [Option(shortName: 'd', longName: "downloadPath", Required = false, HelpText = "The path fot the download folder")]
    public string? pathToDownload { get; set; }
    
    [Option(shortName: 'r', longName: "rootPath", Required = false, HelpText = "The path for the root folder")]
    public string? pathToRoot { get; set; }
}
using CommandLine;

namespace MusicLib.Main;

public class CommandOptions
{
    [Option(shortName: 'd', longName: "downloadPath", Required = false, HelpText = "The path fot the download folder")]
    public string? pathToDownload { get; set; }
    
    [Option(shortName: 'r', longName: "rootPath", Required = false, HelpText = "The path for the root folder")]
    public string? pathToRoot { get; set; }
    
    [Option(shortName: 's', longName: "sort", Required = false, HelpText = "Check for sorting in folders")]
    public bool sortPaths { get; set; }
    
    [Option(longName: "Set-Download-path", Required = false, HelpText = "set the download path as the one provided with -d or --downloadPath")]
    public bool setDownloadPath { get; set; }
    
    [Option(longName: "Set-Root-path", Required = false, HelpText = "set the root path as the one provided with -r or --rootPath")]
    public bool setRootPath { get; set; }
    
    [Option(shortName: 'p', longName: "playlist", Required = false, HelpText = "Create a playlist importable by itunes with the song just downloaded")]
    public bool playlistFlag { get; set; }
}
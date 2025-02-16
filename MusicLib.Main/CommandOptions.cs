using System.ComponentModel.DataAnnotations;
using CommandLine;

namespace MusicLib.Main;

public class CommandOptions
{
    [Option('u', "url", Required = false)]
    public string Url { get; set; }
    
    [Option(shortName: 'd', longName: "download-path", Required = false, HelpText = "The path fot the download folder")]
    public string? PathToDownload { get; set; }
    
    [Option(shortName: 'r', longName: "root-path", Required = false, HelpText = "The path for the root folder")]
    public string? PathToRoot { get; set; }
    
    [Option(shortName: 's', longName: "sort", Required = false, HelpText = "Check for sorting in folders")]
    public bool SortPaths { get; set; }
    
    [Option(longName: "Set-Download-path", Required = false, HelpText = "set the download path as the one provided with -d or --downloadPath")]
    public bool SetDownloadPath { get; set; }
    
    [Option(longName: "Set-Root-path", Required = false, HelpText = "set the root path as the one provided with -r or --rootPath")]
    public bool SetRootPath { get; set; }
    
    [Option(shortName: 'x', longName: "XmlPlaylist", Required = false, HelpText = "Create a playlist in XML format importable by itunes with the song just downloaded")]
    public bool XMLPlaylistFlag { get; set; }
    
    [Option(shortName: 'm', longName: "M3UPlaylist", Required = false, HelpText = "Create a playlist in M3U format importable by itunes with the song just downloaded")]
    public bool M3UPlaylistFlag { get; set; }
    
    [Option(shortName:'n', longName:"playlist-name" , Required = false, HelpText = "The name of the playlist")]
    public string PlaylistName { get; set; }
}
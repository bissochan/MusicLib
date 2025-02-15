using MusicLib.Scripts;
using System;
using Microsoft.Extensions.Configuration;
using CommandLine;
using MusicLib.Main;
using static MusicLib.Main.CommandOptions;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Parser.Default.ParseArguments<CommandOptions>(args).WithParsed(options =>
        {
            if (options.pathToDownload != null)
            {
                config["DownloadDir"] = options.pathToDownload;
                Console.WriteLine("Download Path changed to: "+config["DownloadDir"]);
            }

            if (options.pathToRoot != null)
            {
                config["RootDir"] = options.pathToRoot;
                Console.WriteLine("Root Path changed to: "+config["RootDir"]);
            }
        });

        if (args.Length == 0)
        {
            Console.WriteLine("Please specify at least a valid Spotify playlist url.");
            return;
        }


        string? pathToDownloadFolder = config["DownloadDir"];
        if (pathToDownloadFolder == null)
        {
            Console.WriteLine("Please set a valid download directory. use '-d' to set the download directory.");
        }

        string url = args[0];
        Console.WriteLine("Starting...");
        Console.WriteLine("URL playlist:" + url);

        ScriptManager sManager = new ScriptManager();
        sManager.RunSpotdl(url, pathToDownloadFolder);
    }
}
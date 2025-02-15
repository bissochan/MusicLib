using CommandLine;
using Microsoft.Extensions.Configuration;
using MusicLib.Scripts;
using MusicLib.FileM;
using MusicLib.Playlist;

namespace MusicLib.Main;

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
            if (options.setDownloadPath != null || config["DownloadDir"] == null)
            {
                config["DownloadDir"] = options.pathToDownload;
                Console.WriteLine("download path changed to: " + options.pathToDownload);
            }

            if (options.setRootPath != null || config["RootDir"] == null)
            {
                config["RootDir"] = options.pathToRoot;
                Console.WriteLine("root path changed to: " + options.pathToRoot);
            }

            string? pathToDownloadFolder = options.pathToDownload ?? config["DownloadDir"];
            string? pathToRootFolder = options.pathToRoot ?? config["RootDir"];
            bool flagSort = options.sortPaths;
            bool flagPlaylist= options.playlistFlag;


            if (args.Length == 0)
            {
                Console.WriteLine("Please specify at least a valid Spotify playlist url.");
                return;
            }



            if (pathToDownloadFolder == null)
            {
                Console.WriteLine("Please set a valid download directory. use '-d' to set the download directory.");
                return;
            }

            string url = args[0];
            Console.WriteLine("Starting...");
            Console.WriteLine("URL playlist:" + url);

            ScriptManager sManager = new ScriptManager();
            sManager.RunSpotdl(url, pathToDownloadFolder);

            if (!flagSort)
            {
                return;
            }
            FileManager fManager = new FileManager(pathToRootFolder);
            List<SongData> songData = fManager.AddToLibrary(pathToDownloadFolder);

            if (!flagPlaylist)
            {
                return;
            }
            PlaylistManager pManager = new PlaylistManager(pathToRootFolder);
            pManager.CreatePlaylist(songData);

        });
    }
}
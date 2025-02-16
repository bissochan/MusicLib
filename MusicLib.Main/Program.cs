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
        ;

        Parser.Default.ParseArguments<CommandOptions>(args).WithParsed(options =>
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            //if (options.SetDownloadPath != null || config["DownloadDir"] == null)
            //{
            //    config["DownloadDir"] = options.PathToDownload;
            //    Console.WriteLine("download path changed to: " + options.PathToDownload);
            //}

            //if (options.SetRootPath != null || config["RootDir"] == null)
            //{
            //    config["RootDir"] = options.PathToRoot;
            //    Console.WriteLine("root path changed to: " + options.PathToRoot);
            //}

            string? pathToDownloadFolder = options.PathToDownload ?? config["DownloadDir"];
            string? pathToRootFolder = options.PathToRoot ?? config["RootDir"];
            string url = options.Url;
            string defaultDirectory = config["DefaultDir"];


            if (pathToDownloadFolder == null)
            {
                Console.WriteLine("Please set a valid download directory. use '-d' to set the download directory.");
                return;
            }


            Console.WriteLine("Starting...");
            Console.WriteLine("URL playlist:" + url);

            if (url != null)
            {
                ScriptManager sManager = new ScriptManager();
                sManager.RunSpotdl(url, pathToDownloadFolder);
            }


            if (!options.SortPaths)
            {
                return;
            }

            FileManager fManager = new FileManager(pathToRootFolder, defaultDirectory);
            List<SongData> songData = fManager.AddToLibrary(pathToDownloadFolder);

            if (songData.Count == 0)
            {
                return;
            }

            PlaylistManager pManager = new PlaylistManager(pathToRootFolder, config["PlaylistDir"]);
            if (options.XMLPlaylistFlag)
            {
                
                pManager.CreatePlaylistXML(songData, options.PlaylistName);
            }

            if (options.M3UPlaylistFlag)
            {
                pManager.CreatePlaylistM3U(songData, options.PlaylistName);
            }


            //fManager.Clean();
            Console.WriteLine("Done!");
        });
    }
}
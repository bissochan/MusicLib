using CommandLine;
using Microsoft.Extensions.Configuration;
using MusicLib.FileM;
using MusicLib.Playlist;
using MusicLib.Scripts;
using Newtonsoft.Json;

namespace MusicLib.Main;

class Program
{
    static void Main(string[] args)
    {
        

        Parser.Default.ParseArguments<CommandOptions>(args).WithParsed(options =>
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            if (options.SetDownloadPath)
            {
                if (options.PathToDownload != null)
                {
                    string json = File.ReadAllText("appsettings.json");
                    if (!string.IsNullOrEmpty(json))
                    {
                        Console.WriteLine("ERROR: config file is empty");
                        return;
                    }

                    var temp = JsonConvert.DeserializeObject<Configuration>(json);
                    if (temp != null)
                    {
                        temp.DownloadDir = options.PathToDownload;
                        var dummy = JsonConvert.SerializeObject(temp);
                        Console.WriteLine("Download Directory updated to " + temp.DownloadDir);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: path to download not provided, use --download-path to provide one");
                }
            }

            if (options.SetRootPath)
            {
                if (options.PathToRoot != null)
                {
                    string json = File.ReadAllText("appsettings.json");
                    if (!string.IsNullOrEmpty(json))
                    {
                        Console.WriteLine("ERROR: config file is empty");
                        return;
                    }

                    var temp = JsonConvert.DeserializeObject<Configuration>(json);
                    if (temp != null)
                    {
                        temp.RootDir = options.PathToRoot;
                        string dummy = JsonConvert.SerializeObject(temp);
                        Console.WriteLine("Root Directory updated to " + temp.RootDir);
                    }
                }
                else
                {
                    Console.WriteLine("ERROR: path to root not provided, use --root-path to provide one");
                }
            }

            string? pathToDownloadFolder = options.PathToDownload ?? config["DownloadDir"];
            string? pathToRootFolder = options.PathToRoot ?? config["RootDir"];
            string url = options.Url;
            string? defaultDirectory = config["DefaultDir"];


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

            if (pathToRootFolder != null)
            {
                FileManager fManager = new FileManager(pathToRootFolder, defaultDirectory);
                List<SongData> songData = fManager.AddToLibrary(pathToDownloadFolder);

                if (songData.Count == 0)
                {
                    return;
                }

                PlaylistManager pManager = new PlaylistManager(pathToRootFolder, config["PlaylistDir"]);
                if (options.XMLPlaylistFlag)
                {
                    pManager.CreatePlaylistXml(songData, options.PlaylistName);
                }

                if (options.M3UPlaylistFlag)
                {
                    pManager.CreatePlaylistM3U(songData, options.PlaylistName);
                }
            }


            //fManager.Clean();
            Console.WriteLine("Done!");
        });
    }
}
using System.Collections.Generic;
using System.IO;
using TagLib;
using File = System.IO.File;

namespace MusicLib.FileM;

public class FileManager(string rootDirectory, string defaulDirectory)
{
    private string _rootDirectory = rootDirectory;
    private string _defaultDirectory = defaulDirectory;

    public List<SongData> AddToLibrary(string initialFolder)
    {
        var songs = Directory.GetFiles(initialFolder, "*.mp3").ToList();
        Console.WriteLine($"Found {songs.Count} files");
        var songList = new List<SongData>();
        if (!songs.Any())
        {
            Console.WriteLine("No songs found");
            return songList;
        }

        foreach (var song in songs)
        {
            try
            {
                var file = TagLib.File.Create(song);
                var songData = new
                {
                    title = file.Tag.Title,
                    artist = file.Tag.AlbumArtists,
                    album = file.Tag.Album
                };
                var pathArtist = Path.Combine(this._rootDirectory, FileHelper.SanitizeFileName(songData.artist[0]));

                if (!Directory.Exists(pathArtist))
                {
                    Console.WriteLine("Creating directory for new Artist: " + songData.artist[0]);
                    Directory.CreateDirectory(pathArtist);
                }

                var pathAlbum = Path.Combine(pathArtist, FileHelper.SanitizeFileName(songData.album));

                if (!Directory.Exists(pathAlbum))
                {
                    Console.WriteLine("Creating directory for new Album: " + songData.album);
                    Directory.CreateDirectory(pathAlbum);
                }

                var destinationFilePath = Path.Combine(pathAlbum, Path.GetFileName(song));
                Path.GetFileName(song);
                File.Copy(song, destinationFilePath, true);
                var pathSong = Path.Combine(pathAlbum, songData.title);
                songList.Add(new SongData(songData.title, songData.artist, songData.album, pathSong));
            }
            catch
            {
                var defPath = Path.Combine(defaulDirectory, Path.GetFileName(song));
                File.Copy(song, defPath, true);
                songList.Add(new SongData(
                    Path.GetFileName(song),
                    ["default"],
                    "default",
                    defPath
                ));
            }
        }

        return songList;
    }

    public void Clean()
    {
        var songs = Directory.GetFiles(_rootDirectory, "*.mp3").ToList();
        songs.ForEach(File.Delete);
    }
}
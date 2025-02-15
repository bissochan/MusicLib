using System.Collections.Generic;
using System.IO;
using TagLib;
using File = System.IO.File;

namespace MusicLib.FileM;

public class FileManager(string rootDirectory)
{
    private string _rootDirectory = rootDirectory;

    public List<SongData> AddToLibrary(string initialFolder)
    {
        var songs = Directory.GetFiles(this._rootDirectory, "*.mp3").ToList();
        var songList = new List<SongData>();
        if (!songs.Any())
        {
            Console.WriteLine("No songs found");
            return songList;
        }

        foreach (var song in songs)
        {
            var file = TagLib.File.Create(song);
            var songData = new
            {
                title = file.Tag.Title,
                artist = file.Tag.AlbumArtists,
                album = file.Tag.Album
            };
            var pathArtist = Path.Combine(this._rootDirectory, songData.artist[0]);

            if (!Directory.Exists(pathArtist))
            {
                Console.WriteLine("Creating directory for new Artist: " + songData.artist[0]);
            }

            var pathAlbum = Path.Combine(pathArtist, songData.album);

            if (!Directory.Exists(pathAlbum))
            {
                Console.WriteLine("Creating directory for new Album: " + songData.album);
            }

            File.Copy(song, pathAlbum, true);
            var pathSong = Path.Combine(pathAlbum, songData.title);
            songList.Add(new SongData(songData.title, songData.artist, songData.album, pathSong));
        }
        return songList;
    }
}
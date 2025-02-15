using MusicLib.FileM;

namespace MusicLib.Playlist;

public class PlaylistManager(string rootDirectory)
{
    private string _rootDirectory = rootDirectory;

    public void CreatePlaylistM3U(List<SongData> songData, string? playlistName)
    {
        if (playlistName == null)
        {
            playlistName = new Guid().ToString();
        }

        using (StreamWriter writer = new StreamWriter(_rootDirectory + @"\" + playlistName + ".m3u"))
        {
            writer.WriteLine("#EXTM3U");
            foreach (var song in songData)
            {
                var filePath = _rootDirectory + @"\" + song.Artist + @"\" + song.Album + @"\" + song.Title + ".mp3";
                writer.WriteLine(filePath);
            }
        }
    }
}
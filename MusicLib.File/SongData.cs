namespace MusicLib.FileM;

public class SongData
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Album { get; set; } = string.Empty;
    public string PathToSong { get; set; } = string.Empty;

    public SongData(string title, string[] artist, string? album, string pathToSong)
    {
        Title = title;
        Artist = artist[0];
        Album = album;
        PathToSong = pathToSong;
    }
    
}
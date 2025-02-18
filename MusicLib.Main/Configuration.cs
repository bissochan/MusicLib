namespace MusicLib.Main;

public class Configuration
{
    public Logging Logging { get; set; } = null!;
    public string DownloadDir { get; set; } = null!;
    public string RootDir { get; set; } = null!;
    public string PlaylistDirAlt { get; set; } = null!;
    public string PlaylistDir { get; set; } = null!;
    public string DefaultDir { get; set; } = null!;
}

public class Logging
{
    public LogLevel LogLevel { get; set; } = null!;
}

public class LogLevel
{
    public string Default { get; set; } = null!;
    public string System { get; set; } = null!;
    public string Microsoft { get; set; } = null!;
}
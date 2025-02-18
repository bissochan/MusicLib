namespace MusicLib.Main;

public class Configuration
{
    public Logging Logging { get; set; }
    public string DownloadDir { get; set; }
    public string RootDir { get; set; }
    public string PlaylistDirAlt { get; set; }
    public string PlaylistDir { get; set; }
    public string DefaultDir { get; set; }
}

public class Logging
{
    public LogLevel LogLevel { get; set; }
}

public class LogLevel
{
    public string Default { get; set; }
    public string System { get; set; }
    public string Microsoft { get; set; }
}
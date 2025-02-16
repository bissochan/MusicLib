using TagLib;
using System.Xml;
using MusicLib.FileM;

namespace MusicLib.Playlist
{
    public class PlaylistManager
    {
        private string _rootDirectory;
        private string _playlistDirectory;


        public PlaylistManager(string rootDirectory, string playlistDirectory)
        {
            _rootDirectory = rootDirectory;
            _playlistDirectory = playlistDirectory;
        }


        public void CreatePlaylistM3U(List<SongData> songData, string? playlistName)
        {
            if (string.IsNullOrEmpty(playlistName))
            {
                playlistName = Guid.NewGuid().ToString();
            }


            if (!Directory.Exists(_playlistDirectory))
            {
                Console.WriteLine($"Directory {_playlistDirectory} does not exist. Creating it now.");
                Directory.CreateDirectory(_playlistDirectory);
            }

            string playlistPath = Path.Combine(_playlistDirectory, playlistName + ".m3u");

            using (StreamWriter writer = new StreamWriter(playlistPath))
            {
                writer.WriteLine("#EXTM3U");

                foreach (var song in songData)
                {
                    string filePath = Path.Combine(_rootDirectory, song.Artist, song.Album, song.Title + ".mp3");
                    writer.WriteLine(filePath);
                    Console.WriteLine($"{song.Title} added to playlist {playlistName}");
                }
            }

            Console.WriteLine($"Playlist '{playlistName}.m3u' created successfully at {_playlistDirectory}");
        }

        public void CreatePlaylistXML(List<SongData> songs, string? playlistName)
        {
            var playlistPath = Path.Combine(_playlistDirectory, playlistName + ".xml");
            using (XmlWriter writer = XmlWriter.Create(playlistPath, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteDocType("plist", "-//Apple Computer//DTD PLIST 1.0//EN",
                    "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
                writer.WriteStartElement("plist");
                writer.WriteAttributeString("version", "1.0");
                writer.WriteStartElement("dict");
                writer.WriteStartElement("key", "Tracks");
                writer.WriteStartElement("dict");

                Dictionary<int, string> trackMap = new Dictionary<int, string>();
                int trackId = 1000;

                foreach (var songPathFull in songs)
                {
                    var songPath = songPathFull.PathToSong;
                    string title = Path.GetFileNameWithoutExtension(songPath); // Default: Use file name
                    string artist = "Unknown Artist";
                    string album = "Unknown Album";
                    string genre = "Unknown Genre";
                    int duration = 0; // Default to 0 seconds if not available

                    try
                    {
                        var file = TagLib.File.Create(songPath);
                        title = !string.IsNullOrWhiteSpace(file.Tag.Title) ? file.Tag.Title : title;
                        artist = file.Tag.Performers.Length > 0 && !string.IsNullOrWhiteSpace(file.Tag.Performers[0])
                            ? file.Tag.Performers[0]
                            : artist;
                        album = !string.IsNullOrWhiteSpace(file.Tag.Album) ? file.Tag.Album : album;
                        genre = file.Tag.Genres.Length > 0 && !string.IsNullOrWhiteSpace(file.Tag.Genres[0])
                            ? file.Tag.Genres[0]
                            : genre;
                        duration = (int)file.Properties.Duration.TotalSeconds;
                    }
                    catch (TagLib.UnsupportedFormatException)
                    {
                        Console.WriteLine($"⚠️ Unsupported file format: {songPath} (Added without metadata)");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error processing {songPath}: {ex.Message} (Added without metadata)");
                    }

                    // Add the song to the playlist
                    trackId++;
                    trackMap[trackId] = songPath;

                    writer.WriteElementString("key", trackId.ToString());
                    writer.WriteStartElement("dict");
                    writer.WriteElementString("key", "Track ID");
                    writer.WriteElementString("integer", trackId.ToString());
                    writer.WriteElementString("key", "Name");
                    writer.WriteElementString("string", title);
                    writer.WriteElementString("key", "Artist");
                    writer.WriteElementString("string", artist);
                    writer.WriteElementString("key", "Album");
                    writer.WriteElementString("string", album);
                    writer.WriteElementString("key", "Genre");
                    writer.WriteElementString("string", genre);
                    writer.WriteElementString("key", "Total Time");
                    writer.WriteElementString("integer", duration.ToString());
                    writer.WriteElementString("key", "Location");
                    writer.WriteElementString("string", "file://localhost/" + songPath.Replace("\\", "/"));
                    writer.WriteEndElement(); // End dict for this track
                }


                writer.WriteEndElement();

                writer.WriteElementString("key", "Playlists");
                writer.WriteStartElement("array");

                writer.WriteStartElement("dict");
                writer.WriteElementString("key", "Name");
                writer.WriteElementString("string", playlistName);
                writer.WriteElementString("key", "Playlist Items");
                writer.WriteStartElement("array");

                foreach (var id in trackMap.Keys)
                {
                    writer.WriteStartElement("dict");
                    writer.WriteElementString("key", "Track ID");
                    writer.WriteElementString("integer", id.ToString());
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Console.WriteLine($"Playlist '{playlistName}.xml' created successfully at {_playlistDirectory}");

        }
    }
}
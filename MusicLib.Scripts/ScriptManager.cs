using System;
using System.Diagnostics;

namespace MusicLib.Scripts;

public interface IScriptManager
{
    void RunSpotdl(string urlPlaylist, string pathToFolder);
}

public class ScriptManager : IScriptManager
{
    public void RunSpotdl(string urlPlaylist, string pathToFolder)
    {
        
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "spotdl",
            Arguments = urlPlaylist,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false,
            UseShellExecute = false,
            WorkingDirectory = pathToFolder
        };
        
        Console.WriteLine("Running Spotdl");

        using (Process process = Process.Start(psi)!)
        {
            process.OutputDataReceived += (s, e) => { Console.WriteLine(e.Data); };
            process.ErrorDataReceived += (s, e) => { Console.WriteLine(e.Data); };

            process.Start();
            
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            
            process.WaitForExit();
        }
        
        
        
    }
}
using System;
using System.Diagnostics;

namespace MusicLib.Scripts;

public interface IScriptManager
{
    void RunSpotdl(string urlPlaylist);
}

public class ScriptManager : IScriptManager
{
    public void RunSpotdl(string urlPlaylist)
    {
        
        ProcessStartInfo psi = new ProcessStartInfo()
        {
            FileName = "spotdl",
            Arguments = urlPlaylist,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = false,
            UseShellExecute = false
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
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MusicLib.FileM;

public static class FileHelper
{
    public static string SanitizeFileName(string name, char replacement = '_')
    {
        // Define invalid characters for file and folder names
        char[] invalidChars = Path.GetInvalidFileNameChars();
        
        // Replace each invalid character with the specified replacement character
        foreach (char c in invalidChars)
        {
            name = name.Replace(c, replacement);
        }

        // Trim spaces and dots to prevent issues (Windows doesn't allow trailing dots/spaces in folders)
        return name.Trim().TrimEnd('.');
    }
}
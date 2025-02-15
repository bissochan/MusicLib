using MusicLib.Scripts;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Please specify an argument.");
            return;
        }
        
        string url = args[0];
        Console.WriteLine("Starting...");
        Console.WriteLine("URL playlist:" + url);
        
        ScriptManager sManager = new ScriptManager();
        sManager.RunSpotdl(url);
        
        
    }
}
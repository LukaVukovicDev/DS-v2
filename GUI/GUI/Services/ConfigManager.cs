using System.IO;

public sealed class ConfigManager
{
    private static ConfigManager _instance;
    private static readonly object _lock = new object();

    public string ChainName { get; private set; }
    public string ConnectionString { get; private set; }

    private ConfigManager()
    {
        LoadConfig();
    }

    public static ConfigManager Instance
    {
        get
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new ConfigManager();
                return _instance;
            }
        }
    }

    private void LoadConfig()
    {
        string path = "config.txt";

        if (!File.Exists(path))
            throw new FileNotFoundException("Config file not found!");

        var lines = File.ReadAllLines(path);

        if (lines.Length < 2)
            throw new Exception("Config file format invalid!");

        ChainName = lines[0];
        ConnectionString = lines[1];
    }
}
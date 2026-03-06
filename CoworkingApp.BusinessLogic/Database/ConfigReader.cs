using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoworkingApp.BusinessLogic.Database
{
    public class ConfigReader
    {
        public string ChainName { get; private set; }
        public string ConnectionString { get; private set; }
        
        public ConfigReader(string filePath)
        {
            if(!File.Exists(filePath))
            {
                throw new FileNotFoundException("Configuration file not found. Please make sure config.txt exists.");
            }

            string[] lines = File.ReadAllLines(filePath);

            if(lines.Length < 2)
            {
                throw new InvalidDataException("config.txt is incomplete. It must have exactly 2 lines: chain name and connection string.");
            }

            ChainName = lines[0];
            ConnectionString = lines[1];
        }
    }
}

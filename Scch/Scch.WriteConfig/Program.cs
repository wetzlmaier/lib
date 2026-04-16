using System;
using System.IO;
using Scch.Common;
using Scch.Common.Configuration;

namespace Scch.WriteConfig
{
    class Program
    {
        static int Main(string[] args)
        {
            var configFile = ConfigurationHelper.Current.ReadString("ConfigFile");
            var key = ConfigurationHelper.Current.ReadString("Key");
            var value = ConfigurationHelper.Current.ReadString("Value");

            try
            {
                if (!File.Exists(configFile))
                {
                    Console.WriteLine("File '{0}' does not exist.", configFile);
                }

                var config = ConfigurationHelper.OpenConfiguration(configFile);
                config.SetValue(key, value);
                config.Save();
                Console.WriteLine("Configuration saved.");

                return ExitCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return ExitCodes.Error;
            }
        }
    }
}

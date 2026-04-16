using System;
using System.IO;
using Scch.Common;
using Scch.Common.Configuration;
using Scch.Common.Diagnostics;

namespace Scch.ExecuteParallel
{
    class Program
    {
        static int Main(string[] args)
        {
            var maxDegreeOfParallelism = ConfigurationHelper.Current.ReadInt("MaxDegreeOfParallelism", Environment.ProcessorCount);
            var commandsFile = ConfigurationHelper.Current.ReadString("CommandsFile");

            string[] commands;
            if (commandsFile != null)
            {
                Console.WriteLine("Reading commands from '{0}'.", commandsFile);
                commands = File.ReadAllLines(commandsFile);
            }
            else
            {
                Console.WriteLine("Enter commands:");
                commands = ConsoleApplication.ReadStdIn();
            }

            try
            {
                var output = ProcessHelper.ExecuteBatchProcessesParallel(commands, maxDegreeOfParallelism, true);
                return ExitCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
#if DEBUG
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
#endif
                return ExitCodes.Error;
            }
        }
    }
}

using System;
using System.Collections;
using System.IO;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Runtime;
using Scch.Common;
using Scch.Common.Configuration;
using Scch.Logging;
using Scch.ModelBasedTesting.Osmo;

namespace Scch.ModelBasedTesting.CreateOsmoTestClass
{
    public class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var outputDirectory = ConfigurationHelper.Current.ReadString("OutputDirectory", Environment.CurrentDirectory);
                var template = ConfigurationHelper.Current.ReadString("Template", "Template.java");
                var afterTest = ConfigurationHelper.Current.ReadString("AfterTest");
                var beforeTest = ConfigurationHelper.Current.ReadString("BeforeTest");

                var debug = ConfigurationHelper.Current.ReadBool("Debug");
                var wrapper = new ModelWrapper(ConfigurationHelper.Current.ReadString("ModelType"), debug);
                var assemblyName = Path.GetFileNameWithoutExtension(wrapper.ModelType.Assembly.Location);
                var name = ConfigurationHelper.Current.ReadString("TestClassName", wrapper.ModelType.Name + "Bridge");
                var package = ConfigurationHelper.Current.ReadString("Package", assemblyName.ToLower());
                var modelType = ConfigurationHelper.Current.ReadString("ModelType");

                var testClass = TestClassBuilder.Build(modelType, debug, package, name, beforeTest, afterTest);

                var engine = new VelocityEngine();
                engine.Init();
                engine.SetProperty(RuntimeConstants.INPUT_ENCODING, "UTF-8");
                engine.SetProperty(RuntimeConstants.OUTPUT_ENCODING, "UTF-16");

                IContext context = new VelocityContext(new Hashtable { { "testClass", testClass } });

                using (StreamWriter writer = new StreamWriter(new FileStream(Path.Combine(outputDirectory, testClass.Name + new FileInfo(template).Extension), FileMode.Create)))
                {
                    engine.MergeTemplate(template, "UTF-8", context, writer);
                }

                Console.WriteLine("OSMO test class written successfully.");
                return ExitCodes.Ok;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                Logger.Write(new ExceptionLogEntry(ex));
                return ExitCodes.Error;
            }
        }
    }
}

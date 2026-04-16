using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using Scch.Common.IO;

namespace Scch.Common.ComponentModel.Composition
{
    public static class CompositionHelper
    {
        public static CompositionContainer CreateCompositionContainer(SearchOption searchOption=SearchOption.TopDirectoryOnly)
        {
            return CreateCompositionContainer(new string[0], new string[0], searchOption);
        }

        public static CompositionContainer CreateCompositionContainer(string[] ignoreAssemblies, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return CreateCompositionContainer(new string[0], ignoreAssemblies, searchOption);
        }

        public static CompositionContainer CreateCompositionContainer(string[] moduleAssemblies, string[] ignoreAssemblies, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            ISet<string> assemblies = new HashSet<string>();
            var catalog = new AggregateCatalog();

            var ignore = ignoreAssemblies.Select(assembly => assembly.ToLower()).ToList();

            var executingAssemblyLocation = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            var files = new List<FileInfo>();
            foreach (var fileType in FileHelper.AssemblyFileTypes)
            {
                files.AddRange(executingAssemblyLocation.EnumerateFiles("*" + fileType, searchOption));
            }

            foreach (var file in files)
            {
                if (ignore.Contains(file.Name.ToLower()))
                    continue;

                try
                {
                    Assembly assembly = Assembly.LoadFrom(file.FullName);

                    if (assemblies.Contains(assembly.Location.ToLower()))
                    {
                        throw new InvalidOperationException($"Assembly '{assembly.Location}' is already loaded.");
                    }

                    catalog.Catalogs.Add(new AssemblyCatalog(assembly));
                    assemblies.Add(assembly.Location.ToLower());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (!assemblies.Contains(Assembly.GetExecutingAssembly().Location.ToLower()))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
                assemblies.Add(Assembly.GetExecutingAssembly().Location.ToLower());
            }

            if (Assembly.GetEntryAssembly() != null && !assemblies.Contains(Assembly.GetEntryAssembly().Location.ToLower()))
            {
                catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetEntryAssembly()));
                assemblies.Add(Assembly.GetEntryAssembly().Location.ToLower());
            }

            // Load module assemblies as well (e.g. Reporting extension). See App.config file.
            foreach (string moduleAssembly in moduleAssemblies)
            {
                Assembly assembly = Assembly.LoadFrom(moduleAssembly);
                if (assemblies.Contains(assembly.Location.ToLower()))
                    throw new InvalidOperationException("Assembly is already loaded.");

                catalog.Catalogs.Add(new AssemblyCatalog(moduleAssembly));
            }

            var container = new CompositionContainer(catalog);
            var batch = new CompositionBatch();
            batch.AddExportedValue(container);
            container.Compose(batch);

            return container;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Scch.Common.ComponentModel.Composition
{
    public static class MetadataHelper
    {
        public static T[] GetExports<T>(IEnumerable<Lazy<T, INameMetadata>> plugins, params string[] names)
        {
            return GetExports(plugins, true, names);
        }

        private static T[] GetExports<T>(IEnumerable<Lazy<T, INameMetadata>> plugins, bool checkAvailability, params string[] names)
        {
            var result = new List<T>();
            var enumerable = plugins as Lazy<T, INameMetadata>[] ?? plugins.ToArray();

            foreach (string name in names)
            {
                var exports = (from selected in enumerable where name == selected.Metadata.Name select selected.Value).ToArray();

                if (checkAvailability && !exports.Any())
                    throw new ExportNotFoundException(name);

                result.AddRange(exports);
            }

            return result.ToArray();
        }

        public static T GetExportOrDefault<T>(IEnumerable<Lazy<T, INameMetadata>> plugins, string name)
        {
            return GetExports(plugins, false, name).SingleOrDefault();
        }

        public static T GetExport<T>(IEnumerable<Lazy<T, INameMetadata>> plugins, string name)
        {
            return GetExports(plugins, false, name).Single();
        }
    }
}

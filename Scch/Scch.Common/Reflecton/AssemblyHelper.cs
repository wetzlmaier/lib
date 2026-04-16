using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace Scch.Common.Reflecton
{
    /// <summary>
    /// Helper functions for <see cref="Assembly"/>.
    /// </summary>
    public static class AssemblyHelper
    {
        /// <summary>
        /// File extensions for assemblies.
        /// </summary>
        public static readonly string[] Extensions = { "dll", "exe" };

        /// <summary>
        /// Returns the file name of the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The file name of the specified assembly.</returns>
        public static string GetAssemblyFileName(Assembly assembly)
        {
            return new Uri(assembly.GetName().CodeBase).LocalPath;
        }

        public static string GetApplicationDirectory()
        {
            return new FileInfo(GetAssemblyFileName(Assembly.GetExecutingAssembly())).DirectoryName;
        }

        /// <summary>
        /// Returns the extension of the assembly file.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns>The extension of the assembly file.</returns>
        public static string GetAssemblyFileExtension(Assembly assembly)
        {
            return new FileInfo(GetAssemblyFileName(assembly)).Extension;
        }

        /// <summary>
        /// Loads the application icon.
        /// </summary>
        /// <returns>The application icon.</returns>
        public static Icon LoadApplicationIcon()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                return null;
            using (Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".App.ico"))
            {
                return stream == null ? null : new Icon(stream);
            }
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyCompanyAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyCompanyAttribute"/>.</returns>
        public static string GetAssemblyCompany(Assembly assembly)
        {
            var attribute = (AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));
            return attribute != null ? attribute.Company : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyProductAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyProductAttribute"/>.</returns>
        public static string GetAssemblyProduct(Assembly assembly)
        {
            var attribute = (AssemblyProductAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));
            return attribute != null ? attribute.Product : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyCopyrightAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyCopyrightAttribute"/>.</returns>
        public static string GetAssemblyCopyright(Assembly assembly)
        {
            var attribute = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
            return attribute != null ? attribute.Copyright : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyTrademarkAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyTrademarkAttribute"/>.</returns>
        public static string GetAssemblyTrademark(Assembly assembly)
        {
            var attribute = (AssemblyTrademarkAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTrademarkAttribute));
            return attribute != null ? attribute.Trademark : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyCultureAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyCultureAttribute"/>.</returns>
        public static string GetAssemblyCulture(Assembly assembly)
        {
            var attribute = (AssemblyCultureAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCultureAttribute));
            return attribute != null ? attribute.Culture : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyVersionAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyVersionAttribute"/>.</returns>
        public static string GetAssemblyVersion(Assembly assembly)
        {
            var attribute = (AssemblyVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyVersionAttribute));
            return attribute != null ? attribute.Version : string.Empty;
        }

        /// <summary>
        /// Returns the information from the <see cref="AssemblyFileVersionAttribute"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The information from the <see cref="AssemblyFileVersionAttribute"/>.</returns>
        public static string GetAssemblyFileVersion(Assembly assembly)
        {
            var attribute = (AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute));
            return attribute != null ? attribute.Version : string.Empty;
        }

        /// <summary>
        /// Returns the public key token of an <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to get the information from.</param>
        /// <returns>The public key token of an <see cref="Assembly"/>.</returns>
        public static string GetPublicKeyToken(Assembly assembly)
        {
            var result=new StringBuilder();
            var pk = assembly.GetName().GetPublicKey();
            for (int i = 0; i < pk.GetLength(0); i++)
                result.AppendFormat("{0:x2}", pk[i]);
            return result.ToString();
        }

        public static string GetProcessorArchitecture(Assembly assembly)
        {
            return assembly.GetName().ProcessorArchitecture.ToString();
        }
    }
}

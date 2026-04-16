using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Xsl;

namespace Scch.Common.Xml.Xsl
{
    /// <summary>
    /// Helper class for <see cref="XslCompiledTransform"/>.
    /// </summary>
    public class XslTransformHelper
    {
        /// <summary>
        /// Loads and returns a <see cref="XslCompiledTransform"/> from an <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> of the resource.</param>
        /// <returns>A <see cref="XslCompiledTransform"/> from an <see cref="Uri"/>.</returns>
        public static XslCompiledTransform LoadTransformResource(Uri uri)
        {
            var res = Application.GetResourceStream(uri);

            Debug.Assert(res != null);

            using (var stream = res.Stream)
            {
                return LoadTransform(stream);
            }
        }

        /// <summary>
        /// Loads and returns a <see cref="XslCompiledTransform"/> from a file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>A <see cref="XslCompiledTransform"/> from a file.</returns>
        public static XslCompiledTransform LoadTransform(string file)
        {
            using (var stream = File.OpenRead(file))
            {
                return LoadTransform(stream);
            }
        }

        /// <summary>
        /// Loads and returns a <see cref="XslCompiledTransform"/> from a <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/>.</param>
        /// <returns>A <see cref="XslCompiledTransform"/> from a <see cref="Stream"/>.</returns>
        public static XslCompiledTransform LoadTransform(Stream stream)
        {
            XmlReader xr = XmlReader.Create(stream);
            var xslt = new XslCompiledTransform();
            xslt.Load(xr);
            return xslt;
        }
    }
}

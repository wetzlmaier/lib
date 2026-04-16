using System;
using System.Diagnostics;
using System.Reflection;

namespace Scch.Common.Reflecton
{
    public class AssemblyInfo : IAssemblyInfo
    {
        private readonly FileVersionInfo _info;
        private readonly Assembly _assembly;

        public AssemblyInfo(string file)
        {
            _info = FileVersionInfo.GetVersionInfo(file);

            try
            {
                _assembly = Assembly.LoadFile(file);
            }
            catch (Exception)
            {
            }
        }

        public string Comments
        {
            get { return _info.Comments; }
        }

        public string Company
        {
            get { return _info.CompanyName; }
        }

        public string SpecialBuild
        {
            get { return _info.SpecialBuild; }
        }

        public string Product
        {
            get { return _info.ProductName; }
        }

        public string Copyright
        {
            get { return _info.LegalCopyright; }
        }

        public string Trademark
        {
            get { return _info.LegalTrademarks; }
        }

        public string Culture
        {
            get { return _info.Language; }
        }

        public string Version
        {
            get { return _info.ProductVersion; }
        }

        public string FileVersion
        {
            get { return _info.FileVersion; }
        }

        public string ProcessorArchitecture
        {
            get { return _assembly == null ? "N/A" : AssemblyHelper.GetProcessorArchitecture(_assembly); }
        }

        public string PublicKeyToken
        {
            get { return _assembly == null ? "N/A" : AssemblyHelper.GetPublicKeyToken(_assembly); }
        }
    }
}

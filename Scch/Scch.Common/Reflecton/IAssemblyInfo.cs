namespace Scch.Common.Reflecton
{
    public interface IAssemblyInfo
    {
        string Comments { get; }
        string Company { get; }
        string SpecialBuild { get; }
        string Product { get; }
        string Copyright { get; }
        string Trademark { get; }
        string Culture { get; }
        string Version { get; }
        string FileVersion { get; }
        string ProcessorArchitecture { get; }
        string PublicKeyToken { get; }
    }
}
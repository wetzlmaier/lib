using System.ComponentModel;

namespace Scch.Common.ComponentModel
{
    public interface IExtendedDataErrorInfo : IDataErrorInfo
    {
        bool IsValid { get; }
    }
}

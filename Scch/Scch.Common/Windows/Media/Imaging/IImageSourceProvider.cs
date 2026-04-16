using System.Windows.Media;

namespace Scch.Common.Windows.Media.Imaging
{
    /// <summary>
    /// Provider interface for <see cref="ImageSource"/>.
    /// </summary>
    public interface IImageSourceProvider
    {
        /// <summary>
        /// The <see cref="ImageSource"/>.
        /// </summary>
        ImageSource ImageSource { get; }
    }
}

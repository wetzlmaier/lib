namespace Scch.Mvvm.View
{
    public interface IMainView : IDialogView
    {
        double Left { get; set; }

        double Top { get; set; }

        double Width { get; set; }

        double Height { get; set; }

        bool IsMaximized { get; set; }
    }
}

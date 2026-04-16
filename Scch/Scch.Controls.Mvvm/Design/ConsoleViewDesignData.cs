using System.Windows.Documents;
using Scch.Controls.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.Design
{
    internal class ConsoleViewDesignData : ConsoleViewModel
    {
        public ConsoleViewDesignData()
        {
            Output.Document = new FlowDocument(new Paragraph(new Run("Beispieltext")) { Style = Output.Styles[0].Style });
        }
    }
}

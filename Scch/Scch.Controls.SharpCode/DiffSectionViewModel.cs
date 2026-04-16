using System.Collections.Generic;

namespace Scch.Controls.SharpCode
{
    public class DiffSectionViewModel
    {
        public string DiffSectionHeader { get; set; }
        public List<DiffLineViewModel> LeftDiff { get; set; }
        public List<DiffLineViewModel> RightDiff { get; set; }
    }
}
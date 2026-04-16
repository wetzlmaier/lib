using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ICSharpCode.AvalonEdit;

namespace Scch.Controls.SharpCode
{
    public partial class SideBySideDiff
    {
        const string DiffCmd = "diff -u";

        public static readonly DependencyProperty DiffProperty = DependencyProperty.Register("Diff",
            typeof(string), typeof(SideBySideDiff), new FrameworkPropertyMetadata(OnDiffChanged));

        public SideBySideDiff()
        {
            InitializeComponent();
        }

        private static void OnDiffChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (SideBySideDiff)d;
            var rootGrid = userControl.RootGrid;
            rootGrid.Children.Clear();

            var diffContents = (string)e.NewValue;

            if (!diffContents.Trim().StartsWith(DiffCmd))
            {
                diffContents = DiffCmd + Environment.NewLine + diffContents;
            }

            var allLines = diffContents.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None).ToList();

            for (int i = 0; i < allLines.Count; i++)
            {
                allLines[i] = allLines[i].Trim('\r', '\n');
            }

            var arrayOfIndexes = Enumerable.Range(0, allLines.Count);

            var diffSectionHeaders = allLines.Zip(arrayOfIndexes,
                    (x, index) => new { Item = x, Index = index })
                .Where(x => x.Item.StartsWith(DiffCmd))
                .ToList();

            foreach (var header in diffSectionHeaders)
            {
                var hunkElements = allLines
                    .Skip(header.Index + 1)
                    .TakeWhile(x => !x.StartsWith(DiffCmd))
                    .ToList();

                var chunks = ResolveDiffSections(hunkElements);

                foreach (var chunk in chunks)
                {
                    var row = chunks.IndexOf(chunk);

                    rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    rootGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                    // draw header
                    var textBlock = new TextBlock
                    {
                        Text = chunk.DiffSectionHeader,
                        Style = (Style)userControl.FindResource("ContextHeaderStyle")
                    };

                    Grid.SetRow(textBlock, 2 * row);
                    Grid.SetColumnSpan(textBlock, 2);
                    rootGrid.Children.Add(textBlock);

                    // draw left diff
                    var leftMargin = new DiffInfoMargin { Lines = chunk.LeftDiff };
                    var left = new TextEditor();
                    left.TextArea.LeftMargins.Add(leftMargin);
                    var leftBackgroundRenderer = new DiffLineBackgroundRenderer { Lines = chunk.LeftDiff };
                    left.TextArea.TextView.BackgroundRenderers.Add(leftBackgroundRenderer);
                    left.Text = string.Join("\r\n", chunk.LeftDiff.Select(x => x.Text));

                    Grid.SetRow(left, 2 * row + 1);
                    rootGrid.Children.Add(left);

                    // draw right diff
                    var rightMargin = new DiffInfoMargin { Lines = chunk.RightDiff };
                    var right = new TextEditor();
                    right.TextArea.LeftMargins.Add(rightMargin);
                    var rightBackgroundRenderer = new DiffLineBackgroundRenderer { Lines = chunk.RightDiff };
                    right.TextArea.TextView.BackgroundRenderers.Add(rightBackgroundRenderer);
                    right.Text = string.Join(Environment.NewLine, chunk.RightDiff.Select(x => x.Text));

                    Grid.SetRow(right, 2 * row + 1);
                    Grid.SetColumn(right, 1);
                    rootGrid.Children.Add(right);
                }
            }

            // TODO: introduce highlighting specific sections
        }

        public string Diff
        {
            get { return (string)GetValue(DiffProperty); }
            set { SetValue(DiffProperty, value); }
        }

        static List<DiffSectionViewModel> ResolveDiffSections(IEnumerable<string> hunkElements)
        {
            // TODO: extract file name
            // TODO: track file name changes

            var diffContents = hunkElements.Skip(2).ToList();
            var sectionHeaders = diffContents.Where(x => x.StartsWith("@@ ")).ToList();

            var regex = new Regex(@"\-(?<leftStart>\d{1,})\,(?<leftCount>\d{1,})\s\+(?<rightStart>\d{1,})\,(?<rightCount>\d{1,})", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var sections = new List<DiffSectionViewModel>();

            foreach (var header in sectionHeaders)
            {
                var lineNumbers = regex.Match(header);
                var startIndex = diffContents.IndexOf(header);
                var innerDiffContents = diffContents.Skip(startIndex + 1).ToList();

                var leftStart = int.Parse(lineNumbers.Groups["leftStart"].Value);
                var leftDiffSize = int.Parse(lineNumbers.Groups["leftCount"].Value);
                var rightStart = int.Parse(lineNumbers.Groups["rightStart"].Value);
                var rightDiffSize = int.Parse(lineNumbers.Groups["rightCount"].Value);

                var leftLineNumbers = Enumerable.Range(leftStart, leftDiffSize)
                    .Select(x => x.ToString(CultureInfo.InvariantCulture));

                var section = new DiffSectionViewModel();
                section.DiffSectionHeader = header;

                // left section - all context + deletes
                section.LeftDiff = innerDiffContents
                    .Where(x => !x.StartsWith("+"))
                    .Zip(leftLineNumbers, (x, line) => new { Item = x, LineNumber = line })
                    .Select(x => DiffLineViewModel.Create(x.LineNumber, x.Item))
                    .ToList();

                // right section - all context + adds
                var rightLineNumbers = Enumerable.Range(rightStart, rightDiffSize)
                    .Select(x => x.ToString(CultureInfo.InvariantCulture));

                section.RightDiff = innerDiffContents
                    .Where(x => !x.StartsWith("-"))
                    .Zip(rightLineNumbers, (x, line) => new { Item = x, LineNumber = line })
                    .Select(x => DiffLineViewModel.Create(x.LineNumber, x.Item))
                    .ToList();

                var missingRowCount = Math.Abs(section.LeftDiff.Count - section.RightDiff.Count);

                if (section.LeftDiff.Count > section.RightDiff.Count)
                {
                    var lastAdd = section.RightDiff.LastOrDefault(x => x.Style == DiffContext.Added);
                    // magic number 3 here is the number of context rows
                    var lastIndex = lastAdd == null ? 2 : section.RightDiff.IndexOf(lastAdd);
                    for (int i = 0; i < missingRowCount; i++)
                    {
                        var missing = new DiffLineViewModel();
                        missing.Style = DiffContext.Blank;
                        missing.Text = "";
                        missing.PrefixForStyle = "";
                        section.RightDiff.Insert(lastIndex + 1, missing);
                    }
                }
                else
                {
                    var lastRemove = section.LeftDiff.LastOrDefault(x => x.Style == DiffContext.Deleted);
                    // magic number 3 here is the number of context rows
                    var lastIndex = lastRemove == null ? 2 : section.LeftDiff.IndexOf(lastRemove);
                    for (int i = 0; i < missingRowCount; i++)
                    {
                        var missing = new DiffLineViewModel();
                        missing.Style = DiffContext.Blank;
                        missing.Text = "";
                        missing.PrefixForStyle = "";
                        section.LeftDiff.Insert(lastIndex + 1, missing);
                    }
                }

                sections.Add(section);

            }

            return sections;
        }

        static string StripOldValues(string s)
        {
            if (s.StartsWith("- "))
                return "";

            return s;
        }

        static string StripNewValues(string s)
        {
            if (s.StartsWith("+ "))
                return "";

            return s;
        }
    }
}

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Media;
using Scch.Common.ComponentModel;
using Scch.Controls.Properties;
using Scch.Mvvm.ViewModel;

namespace Scch.Controls.Mvvm.ViewModel
{
    public class TablePropertiesViewModel : ViewModelBase, IDisplayNamesProvider
    {
        private int _columnCount;
        private int _rowCount;
        private int _columnWidth;
        private bool _autoWidth;
        private SolidColorBrush _borderBrush;
        private Thickness _borderThickness;

        public TablePropertiesViewModel()
            : base(Resources.TablePropertiesViewModel_DisplayName, 0)
        {
            DisplayNames = LocalizedAttributeHelper.CreateDisplayNameDictionary<TablePropertiesViewModel>();
            _columnCount = 1;
            _rowCount = 1;
            _columnWidth = 100;
            _autoWidth = true;
            _borderBrush = Brushes.Black;
            _borderThickness = new Thickness(1);
        }

        [Browsable(false)]
        public IDictionary<string, string> DisplayNames { get; private set; }

        [Range(1, int.MaxValue, ErrorMessageResourceName = "TablePropertiesViewModelColumnCountRange", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "ColumnCount", typeof(Resources))]
        public int ColumnCount
        {
            get
            {
                return _columnCount;
            }
            set
            {
                if (_columnCount == value)
                    return;
                _columnCount = value;
                RaisePropertyChanged(() => ColumnCount);
            }
        }


        [Range(1, int.MaxValue, ErrorMessageResourceName = "TablePropertiesViewModelRowCountRange", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "RowCount", typeof(Resources))]
        public int RowCount
        {
            get
            {
                return _rowCount;
            }
            set
            {
                if (_rowCount == value)
                    return;
                _rowCount = value;
                RaisePropertyChanged(() => RowCount);
            }
        }

        [Range(1, int.MaxValue, ErrorMessageResourceName = "TablePropertiesViewModelColumnWidthRange", ErrorMessageResourceType = typeof(Resources))]
        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "ColumnWidth", typeof(Resources))]
        public int ColumnWidth
        {
            get
            {
                return _columnWidth;
            }
            set
            {
                if (_columnWidth == value)
                    return;
                _columnWidth = value;
                RaisePropertyChanged(() => ColumnWidth);
            }
        }

        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "AutoWidth", typeof(Resources))]
        public bool AutoWidth
        {
            get
            {
                return _autoWidth;
            }
            set
            {
                if (_autoWidth == value)
                    return;
                _autoWidth = value;
                RaisePropertyChanged(() => AutoWidth);
            }
        }

        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "BorderBrush", typeof(Resources))]
        public SolidColorBrush BorderBrush
        {
            get
            {
                return _borderBrush;
            }
            set
            {
                if (Equals(_borderBrush, value))
                    return;
                _borderBrush = value;
                RaisePropertyChanged(() => BorderBrush);
            }
        }

        [LocalizedDisplayName(typeof(TablePropertiesViewModel), "BorderThickness", typeof(Resources))]
        public Thickness BorderThickness
        {
            get
            {
                return _borderThickness;
            }
            set
            {
                if (_borderThickness == value)
                    return;
                _borderThickness = value;
                RaisePropertyChanged(() => BorderThickness);
            }
        }
    }
}

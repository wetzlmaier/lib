using System;
using System.Data;
using System.Windows.Forms;

namespace Scch.ModelBasedTesting.Stepper
{
    public partial class SelectionForm : Form
    {
        private DataTable _dataTable;

        public SelectionForm()
        {
            InitializeComponent();
        }

        private void SelectionForm_Load(object sender, EventArgs e)
        {

        }

        public DataTable DataTable
        {
            get { return _dataTable; }
            set
            {
                if (_dataTable == value)
                    return;

                _dataTable = value;

                foreach (DataColumn column in _dataTable.Columns)
                {
                    var header = new ColumnHeader { Text = column.Caption };
                    lvCombinations.Columns.Add(header);
                }

                lvCombinations.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                foreach (DataRow row in _dataTable.Rows)
                {
                    ListViewItem item = new ListViewItem(row[0].ToString());
                    for (int i = 1; i < _dataTable.Columns.Count; i++)
                    {
                        item.SubItems.Add(row[i].ToString());
                    }

                    lvCombinations.Items.Add(item);
                }

                if (lvCombinations.Items.Count > 0)
                {
                    SelectItem(0);
                }
            }
        }

        private void SelectItem(int index)
        {
            lvCombinations.Items[index].Selected = true;
            lvCombinations.Select();
        }

        public int SelectedIndex
        {
            get
            {
                return lvCombinations.SelectedIndices[0];
            }
            set
            {
                SelectItem(value);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult=DialogResult.OK;
            Close();
        }

        private void lvCombinations_DoubleClick(object sender, EventArgs e)
        {
            AcceptButton.PerformClick();
        }
    }
}

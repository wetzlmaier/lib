using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Scch.Common.Configuration;

namespace Scch.ModelBasedTesting.Stepper
{
    public partial class MainForm : Form
    {
        private readonly IDictionary<string, Button> _buttons;
        private readonly ModelWrapper _wrapper;
        private readonly StringWriter _consoleOutput;
        private const int MaxRows = 20;

        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);

        public MainForm()
        {
            InitializeComponent();

            _consoleOutput = new StringWriter();
            _buttons = new Dictionary<string, Button>();
            _wrapper = new ModelWrapper(ConfigurationHelper.Current.ReadString("ModelType"), ConfigurationHelper.Current.ReadBool("Debug"));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblState.Visible = true;
            txtConsole.Visible = false;
            Console.SetOut(_consoleOutput);

            Text = $"Model Stepper: {_wrapper.ModelType.AssemblyQualifiedName}";

            SuspendLayout();

            var buttons = CreateButtons(_wrapper.Actions.Keys, Button_Click);
            var rows = Math.Min(buttons.Count, MaxRows);

            pnlButtons.ColumnStyles.Clear();
            for (int column = 0; column < Math.Ceiling((double)buttons.Count / rows); column++)
            {
                pnlButtons.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 0));
            }

            pnlButtons.RowStyles.Clear();
            for (int row = 0; row < rows; row++)
            {
                pnlButtons.RowStyles.Add(new RowStyle(SizeType.Percent, (float)1 / rows * 100));
            }

            int width = 0;
            var size = new SizeF(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Width);
            for (int i = 0; i < buttons.Count; i++)
            {
                var button = buttons[i];
                var column = i / rows;
                var row = i % rows;
                pnlButtons.Controls.Add(button, column, row);
                _buttons.Add(button.Name, button);

                using (var g = Graphics.FromHwnd(button.Handle))
                {
                    width = Math.Max(width, (int)g.MeasureString(button.Text, button.Font, size).Width);
                }
            }

            Location = new Point(0, 0);
            Size = new Size((width + 30) * pnlButtons.ColumnStyles.Count, Screen.PrimaryScreen.WorkingArea.Height);

            PerformGuardsAndUpdateState();

            ResumeLayout();

            SetWindowPos(Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        private void PerformGuardsAndUpdateState()
        {
            var output = _consoleOutput.ToString();

            if (!string.IsNullOrEmpty(output))
            {
                txtConsole.Visible = true;
                txtConsole.AppendText(output);
                txtConsole.ScrollToCaret();
                StringBuilder sb = _consoleOutput.GetStringBuilder();
                sb.Remove(0, sb.Length);
            }

            var state = _wrapper.State;
            if (state != null)
            {
                lblState.Visible = true;
                lblState.Text = $"{state}";
            }

            var enabledActions = _wrapper.EnabledActions;

            foreach (var action in _wrapper.Actions.Keys)
            {
                var button = _buttons[action];
                button.Enabled = enabledActions.ContainsKey(action);
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            try
            {
                button.ForeColor = Color.Red;
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();

                var action = button.Name;
                var combinations = _wrapper.Actions[action];

                int selectedCombination = 0;
                if (combinations.Length > 1)
                {
                    using (var dialog = new SelectionForm())
                    {
                        dialog.DataTable = _wrapper.CreateDataTable(action);
                        dialog.SelectedIndex = selectedCombination;

                        var result = dialog.ShowDialog(this);

                        if (result == DialogResult.Cancel)
                            return;

                        selectedCombination = dialog.SelectedIndex;
                    }
                }

                _wrapper.PerformAction(button.Name, combinations[selectedCombination]);

                PerformGuardsAndUpdateState();
            }
            catch (Exception ex)
            {
                while (ex != null && ex.GetType() == typeof(TargetInvocationException))
                    ex = ex.InnerException;

                MessageBox.Show(ex.Message, "Exception");
            }
            finally
            {
                button.ForeColor = Color.FromKnownColor(KnownColor.ControlText);
                Cursor = Cursors.Default;
            }
        }

        private static List<Button> CreateButtons(ICollection<string> actions, EventHandler clickHandler)
        {
            List<Button> buttons = new List<Button>();

            foreach (var action in actions)
            {
                var button = new Button();
                button.SuspendLayout();
                button.Name = action;
                button.Text = action;
                button.Dock = DockStyle.Fill;
                button.Margin = new Padding(0);
                button.Click += clickHandler;
                buttons.Add(button);
            }

            return buttons;
        }
    }
}

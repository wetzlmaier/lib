using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Scch.Common.Windows;

namespace Scch.Logging.UdpLogger
{
    /// <summary>
    /// Main window for the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Members
        readonly int _port = 1234;
        readonly IPAddress _address = IPAddress.Any;

        Thread _udpThread;
        UdpClient _udpSock;
        #endregion Members

        #region Constructors
        /// <summary>
        /// Creates a new instance of <see cref="MainForm"/>.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            WindowOnTop_Click(this, EventArgs.Empty);
        }

        /// <summary>
        /// Creates a new instance of <see cref="MainForm"/>.
        /// </summary>
        /// <param name="address">The ip address to listen for udp datagrams.</param>
        /// <param name="port">The port to listen for udp datagrams.</param>
        public MainForm(IPAddress address, int port)
            : this()
        {
            _address = address;
            _port = port;
        }
        #endregion Constructors

        #region Methods
        private void Receive()
        {
            try
            {
                var sender = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    try
                    {
                        byte[] rawData = _udpSock.Receive(ref sender);
                        string data = Encoding.UTF8.GetString(rawData, 0, rawData.Length);
                        string senderHostName = Dns.GetHostEntry(sender.Address).HostName;

                        BeginInvoke((ThreadStart)delegate
                                                     {
                                                         if (mnuActive.Checked)
                                                         {
                                                             var hostPattern = new Regex(txtHostFilter.Text);
                                                             var dataPattern = new Regex(txtDataFilter.Text);
                                                             if (hostPattern.IsMatch(senderHostName) && dataPattern.IsMatch(data))
                                                             {
                                                                 WindowHelper.FlashWindow(Handle, 1);

                                                                 txtLog.AppendText(string.Format("[{0} {1}]-[{2}] {3}",
                                                                                                 DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(),
                                                                                                 senderHostName, data));
                                                             }
                                                         }
                                                     });

                        Thread.Sleep(10);
                    }
                    catch (SocketException se)
                    {
                        BeginInvoke((ThreadStart)delegate
                                                     {
                                                         stripStatus.Text = "Socketfehler: " + se.Message;
                                                     });
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
        #endregion Methods

        #region EventHandler
        /// <summary>
        /// <see cref="Form.OnClosed"/>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            try
            {
                if (_udpThread.IsAlive)
                {
                    _udpThread.Abort();
                    _udpSock.Close();
                    _udpThread.Join(5000);
                }
            }
            finally
            {
                base.OnClosed(e);
            }
        }

        private void Test_Click(object sender, EventArgs e)
        {
            if (!mnuActive.Checked)
            {
                Active_Click(sender, e);
            }

            var testSock = new UdpClient();
            byte[] data = Encoding.UTF8.GetBytes("Test" + Environment.NewLine);
            testSock.Send(data, data.Length, new IPEndPoint(IPAddress.Loopback, _port));
            //Microsoft.Practices.EnterpriseLibrary.Logging.Logger.Write("Message", "Category", 0, 0);
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            if (txtLog.SelectionLength > 0)
                Clipboard.SetText(txtLog.SelectedText);
            else
                Clipboard.SetText(txtLog.Text);
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            txtLog.SelectAll();
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void Active_Click(object sender, EventArgs e)
        {
            mnuActive.Checked = !mnuActive.Checked;
            btnActive.Checked = !btnActive.Checked;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text = string.Format("UDP Logger ({0}:{1})", _address, _port);
            // initialize and start the receiving thread
            var endPoint = new IPEndPoint(_address, _port);
            _udpSock = new UdpClient(endPoint);

            //Starting the UDP Server thread.
            _udpThread = new Thread(Receive);
            _udpThread.Start();

            /*
            this.Location=new Point(GetIntSetting(keyMainForm + "." + keyLeft, 0), GetIntSetting(keyMainForm + "." + keyTop, 0));
            this.Size=new Size(GetIntSetting(keyMainForm + "." + keyWidth, 640), GetIntSetting(keyMainForm + "." + keyHeight, 480));
             * */

            Location = new Point(0, Screen.PrimaryScreen.Bounds.Height / 2);
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height / 2);
        }
        #endregion EventHandler

        private void WindowOnTop_Click(object sender, EventArgs e)
        {
            mnuWindowOnTop.Checked = !mnuWindowOnTop.Checked;
            WindowHelper.SetTopMostWindow(Handle, mnuWindowOnTop.Checked);
        }
    }
}
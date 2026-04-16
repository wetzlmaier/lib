using System;
using System.Net;
using System.Windows.Forms;

namespace Scch.Logging.UdpLogger
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            IPAddress address = IPAddress.Any;
            int port = 1234;

            try
            {
                foreach (string arg in args)
                {
                    int pos = arg.IndexOf("=");
                    string key;
                    string value;

                    if (pos >= 1)
                    {
                        key = arg.Substring(1, pos - 1);
                        value = arg.Substring(pos + 1);
                    }
                    else
                    {
                        key = arg.Substring(1);
                        value = null;
                    }

                    switch (key.ToLower())
                    {
                        case "?":
                            string usage=
                                "/address=<ip-address>" + Environment.NewLine +
                                "/port=<port>" + Environment.NewLine +
                                "/? Shows this information";
                            MessageBox.Show(usage, "Usage", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return 0;

                        case "port": 
                            if (string.IsNullOrEmpty(value))
                                throw new ArgumentException("port");

                            port = int.Parse(value); 
                            break;
                        
                        case "address":
                            if (string.IsNullOrEmpty(value))
                                throw new ArgumentException("address");

                                address = IPAddress.Parse(value); 
                            break;
                        
                        default: 
                            throw new NotSupportedException(string.Format("Unknown parameter {0}.", key));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 1;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(address, port));
            return 0;
        }
    }
}
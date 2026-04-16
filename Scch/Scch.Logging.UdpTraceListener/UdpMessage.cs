using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Scch.Common;

namespace Scch.Logging.UdpTraceListener
{
    /// <summary>
    /// Represents a <see cref="UdpMessage"/>.
    /// UDP message with functions to accept a LogEntry, Formatting, and sending
    /// </summary>
    public class UdpMessage
    {
        #region Members
        readonly ILogFormatter _formatter;
        readonly LogEntry _logEntry;
        readonly string _remoteHosts;
        readonly string _header;
        readonly string _footer;
        #endregion Members

        #region Constructors
        /// <summary>
        /// Initializes a <see cref="UdpMessage"/>.
        /// </summary>
        /// <param name="remoteHosts">Comma separated list of remote hosts.</param>
        /// <param name="header">The header of the message.</param>
        /// <param name="footer">The footer of the message.</param>
        /// <param name="logEntry">The LogEntry <see cref="LogEntry"/> to send via UDP.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
        /// UDP message should be formatted</param>
        public UdpMessage(string remoteHosts, string header, string footer, LogEntry logEntry, ILogFormatter formatter)
        {
            _remoteHosts = remoteHosts;
            _header = header;
            _footer = footer;

            _logEntry = logEntry;
            _formatter = formatter;
        }

        /// <summary>
        /// Initializes a <see cref="UdpMessage"/>.
        /// </summary>
        /// <param name="remoteHosts">Comma separated list of remote hosts.</param>
        /// <param name="header">The header of the message.</param>
        /// <param name="footer">The footer of the message.</param>
        /// <param name="message">Represents the message to send via UDP.</param>
        /// <param name="formatter">The Formatter <see cref="ILogFormatter"/> which determines how the 
        /// UDP message should be formatted</param>
        public UdpMessage(string remoteHosts, string header, string footer, string message, ILogFormatter formatter)
            : this(remoteHosts, header, footer, new LogEntry(), formatter)
        {
            _logEntry.Message = message;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Send the UDP message
        /// </summary>
        public void Send()
        {
            string[] hosts;
            int[] ports;

            try
            {
                var hostList = new List<string>();
                var portList = new List<int>();

                foreach (string host in _remoteHosts.Split(StringHelper.CommaSemicolon))
                {
                    if (host.Trim() != string.Empty)
                    {
                        string[] temp = host.Split(':');
                        hostList.Add(temp[0].Trim());
                        portList.Add(int.Parse(temp[1].Trim()));
                    }
                }

                hosts = hostList.ToArray();
                ports = portList.ToArray();
            }
            catch (Exception e)
            {
                throw new Exception("Configuration data was unable to be retrieved.  Please configure Hosts, etc.", e);
            }

            for (int receiver = 0; receiver < hosts.Length; receiver++)
            {
                using (var client = new UdpClient(hosts[receiver], ports[receiver]))
                {
                    var sb = new StringBuilder(string.Empty);

                    if (!string.IsNullOrEmpty(_header))
                    {
                        sb.AppendLine(_header);
                    }

                    sb.AppendLine((_formatter != null) ? _formatter.Format(_logEntry) : _logEntry.Message);

                    if (!string.IsNullOrEmpty(_footer))
                    {
                        sb.AppendLine(_footer);
                    }

                    client.Send(Encoding.UTF8.GetBytes(sb.ToString()), sb.Length);
                }
            }
        }
        #endregion Methods
    }
}
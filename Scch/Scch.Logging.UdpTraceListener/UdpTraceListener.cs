/*
 * -----------------------------------------------------------------------------
 * Copyright (C) 
 *    apro Plank & Pressl GmbH
 *    Software Competence Center Hagenberg (SCCH) GmbH
 * All rights reserved.
 * -----------------------------------------------------------------------------
 * This document contains proprietary information belonging to the Copyright 
 * holders. Passing on and copying of this document, use and communication of 
 * its contents is not permitted without prior written authorisation.
 * -----------------------------------------------------------------------------
 * Created on 29.01.2008 by Wetzlmaier
 */

using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Scch.Logging.UdpTraceListener
{
    /// <summary>
    /// A <see cref="TraceListener"/> that writes an UDP message, formatting the output with an <see cref="ILogFormatter"/>.
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class UdpTraceListener : CustomTraceListener 
    {
        /// <summary>
        /// Sends an UDP message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the UDP message</param>
        public override void Write(string message)
        {
            var udpMessage = new UdpMessage(Attributes["remoteHosts"], Attributes["header"], Attributes["footer"], message, Formatter);
            udpMessage.Send();
        }

        /// <summary>
        /// Sends an UDP message given a predefined string
        /// </summary>
        /// <param name="message">The string to write as the UDP message</param>
        public override void WriteLine(string message)
        {
            Write(message);
        }


        /// <summary>
        /// Delivers the trace data as an UDP message.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry)
            {
                var message = new UdpMessage(Attributes["remoteHosts"], Attributes["header"], Attributes["footer"], data as LogEntry, Formatter);
                message.Send();
            }
            else if (data is string)
            {
                Write(data as string);
            }
            else
            {
                base.TraceData(eventCache, source, eventType, id, data);
            }
        }

        /// <summary>
        /// Declare the supported attributes for <see cref="UdpTraceListener"/>
        /// </summary>
        protected override string[] GetSupportedAttributes()
        {
            return new[] { "remoteHosts", "header", "footer" };
        }
    }
}
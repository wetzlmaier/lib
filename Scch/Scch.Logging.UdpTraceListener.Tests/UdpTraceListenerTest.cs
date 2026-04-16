using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Scch.Logging.UdpTraceListener.Tests
{
    [TestClass]
    public class UdpTraceListenerTest
    {
        const int Port = 1234;
        Thread _udpThread;
        UdpClient _udpSock;
        string _data;

        [TestInitialize]
        public void SetUp()
        {
            _data = null;

            // initialize and start the receiving thread
            var endPoint = new IPEndPoint(IPAddress.Any, Port);
            _udpSock = new UdpClient(endPoint);

            //Starting the UDP Server thread.
            _udpThread = new Thread(Receive);
            _udpThread.Start();
        }

        [TestCleanup]
        public void TearDown()
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
            catch
            {
            }            
        }

        [TestMethod]
        public void TestException()
        {
            try
            {
                throw new ArgumentNullException();
            }
            catch (Exception ex)
            {
                Logger.Write(new ExceptionLogEntry(ex));
            }

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains("ArgumentNullException"));
        }

        [TestMethod]
        public void TestDebug()
        {
            const string msg = "Debug message";
            Logger.Write(new DebugLogEntry(msg));

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains(msg));
        }

        [TestMethod]
        public void TestTrace()
        {
            const string msg = "Debug message";
            Logger.Write(new TraceLogEntry(msg));

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains(msg));
        }

        [TestMethod]
        public void TestPerformance()
        {
            DateTime start = DateTime.Now;
            Thread.Sleep(123);
            DateTime done = DateTime.Now;
            Logger.Write(new PerformanceLogEntry(start, done));

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains("TestPerformance"));
        }

        [TestMethod]
        public void TestGeneral()
        {
            const string msg = "General message";
            Logger.Write(new GeneralLogEntry(msg));

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains(msg));
        }

        [TestMethod]
        public void TestSecurity()
        {
            const string msg = "Security message";
            Logger.Write(new SecurityLogEntry(msg));

            Thread.Sleep(100);
            Assert.IsTrue(_data.Contains(msg));
        }

        void Receive()
        {
            try
            {
                var sender = new IPEndPoint(IPAddress.Any, 0);

                while (true)
                {
                    byte[] rawData = _udpSock.Receive(ref sender);
                    string data = Encoding.UTF8.GetString(rawData, 0, rawData.Length);

                    lock (this)
                    {
                        _data = data;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
        }
    }
}

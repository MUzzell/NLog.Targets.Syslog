// Licensed under the BSD license
// See the LICENSE file in the project root for more information

using System;
using System.ComponentModel;

namespace NLog.Targets.Syslog.Settings
{
    /// <summary>TCP configuration</summary>
    public class TcpConfig : NotifyPropertyChanged, IDisposable
    {
        private const string Localhost = "localhost";
        private const int DefaultPort = 514;
        private const int DefaultReconnectInterval = 500;
        private const int DefaultConnectionCheckTimeout = 500000;
        private const int DefaultBufferSize = 4096;
        private string server;
        private int port;
        private TimeSpan recoveryTime;
        private KeepAliveConfig keepAlive;
        private readonly PropertyChangedEventHandler keepAlivePropsChanged;
        private int connectionCheckTimeout;
        private TlsConfig tls;
        private readonly PropertyChangedEventHandler tlsPropsChanged;
        private FramingMethod framing;
        private int dataChunkSize;

        /// <summary>The IP address or hostname of the Syslog server</summary>
        public string Server
        {
            get { return server; }
            set { SetProperty(ref server, value); }
        }

        /// <summary>The port number the Syslog server is listening on</summary>
        public int Port
        {
            get { return port; }
            set { SetProperty(ref port, value); }
        }

        /// <summary>The time interval, in milliseconds, after which a connection is retried</summary>
        public int ReconnectInterval
        {
            get { return recoveryTime.Milliseconds; }
            set { SetProperty(ref recoveryTime, TimeSpan.FromMilliseconds(value)); }
        }

        /// <summary>KeepAlive configuration</summary>
        public KeepAliveConfig KeepAlive
        {
            get { return keepAlive; }
            set { SetProperty(ref keepAlive, value); }
        }

        /// <summary>The time, in microseconds, to wait for a response when checking the connection status</summary>
        public int ConnectionCheckTimeout
        {
            get { return connectionCheckTimeout; }
            set { SetProperty(ref connectionCheckTimeout, value); }
        }

        /// <summary>Tls configuration</summary>
        public TlsConfig Tls
        {
            get { return tls; }
            set { SetProperty(ref tls, value); }
        }

        /// <summary>Which framing method to use</summary>
        /// <remarks>If <see cref="Tls">is enabled</see> get will always return OctetCounting (RFC 5425)</remarks>
        public FramingMethod Framing
        {
            get { return Tls.Enabled ? FramingMethod.OctetCounting : framing; }
            set { SetProperty(ref framing, value); }
        }

        /// <summary>The size of chunks in which data is split to be sent over the wire</summary>
        public int DataChunkSize
        {
            get { return dataChunkSize; }
            set { SetProperty(ref dataChunkSize, value); }
        }

        /// <summary>Builds a new instance of the TcpProtocolConfig class</summary>
        public TcpConfig()
        {
            server = Localhost;
            port = DefaultPort;
            recoveryTime = TimeSpan.FromMilliseconds(DefaultReconnectInterval);
            keepAlive = new KeepAliveConfig();
            keepAlivePropsChanged = (sender, args) => OnPropertyChanged(nameof(KeepAlive));
            keepAlive.PropertyChanged += keepAlivePropsChanged;
            connectionCheckTimeout = DefaultConnectionCheckTimeout;
            Tls = new TlsConfig();
            tlsPropsChanged = (sender, args) => OnPropertyChanged(nameof(KeepAlive));
            tls.PropertyChanged += tlsPropsChanged;
            framing = FramingMethod.OctetCounting;
            dataChunkSize = DefaultBufferSize;
        }

        public void Dispose()
        {
            keepAlive.PropertyChanged -= keepAlivePropsChanged;
            tls.PropertyChanged -= tlsPropsChanged;
        }
    }
}
using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace SwipeFlash.Core
{
    public class HttpEndpointChecker
    {
        #region Public Properties

        /// <summary>
        /// The endpoint to ping
        /// </summary>
        private IPAddress EndpointIP { get; set; }

        /// <summary>
        /// The name of the host
        /// </summary>
        private string Hostname { get; set; }

        /// <summary>
        /// The interval between checks in milliseconds
        /// </summary>
        private int CheckInterval { get; set; }

        /// <summary>
        /// The action to execute when the result is received
        /// </summary>
        private Action<bool> StateChangedCallback { get; set; }

        /// <summary>
        /// Whether a call has not yet been issued
        /// </summary>
        private bool IsFirstLoad { get; set; } = true;

        /// <summary>
        /// The last state of the response
        /// </summary>
        private bool LastResponse { get; set; } = true;

        /// <summary>
        /// Whether the checker is currently running
        /// </summary>
        private bool IsCheckerRunning { get; set; } = false;

        private bool _runChecker = true;
        /// <summary>
        /// Public flag indicating whether the checker should continue running
        /// </summary>
        public bool RunChecker
        {
            get => _runChecker;
            set {
                _runChecker = value;
                // If the checker is set to run 
                // and is not running already
                if (value && !IsCheckerRunning)
                    StartChecker();
            }
        }

        #endregion

        #region Constructor

        public HttpEndpointChecker(string endpoint, int interval, Action<bool> stateChangedCallback)
        {
            CheckInterval = interval;
            StateChangedCallback = stateChangedCallback;
            Hostname = endpoint;

            // Gets the IP address from the host name
            EndpointIP = GetIPAddress(endpoint);
            
            // Starts the checker
            StartChecker();
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Gets the IP address from the host name
        /// </summary>
        /// <param name="hostname">The host name</param>
        /// <returns>The IP address</returns>
        private IPAddress GetIPAddress(string hostname)
        {
            // Initializes the endpoint IP
            var address = IPAddress.None;

            // Gets the IP Address of the endpoint name
            try { address = Dns.GetHostAddresses(hostname)[0]; }
            catch {
                StateChangedCallback(false);
                Debugger.Log(0, "error", "Couldn't resolve host\n");
            }

            return address;
        }

        /// <summary>
        /// Starts the endpoint checker loop
        /// </summary>
        private void StartChecker()
        {
            // Sets the running flag
            IsCheckerRunning = true;

            // Runs the checking task
            Task.Run(async () =>
            {
                // While the checker is active
                while (RunChecker)
                {
                    // Gets the state of the response
                    var isResponding = await IsEndpointRespondingAsync();

                    // If the response is not the same as the last response
                    if (isResponding || isResponding != LastResponse || IsFirstLoad)
                    {
                        // Call the state changed callback
                        StateChangedCallback(isResponding);
                        // Set the last response value
                        LastResponse = isResponding;
                        // Disables the first load flag
                        IsFirstLoad = false;
                    }

                    // Waits for the check interval
                    if (RunChecker) await Task.Delay(CheckInterval);
                }

                // Sets the running flag
                IsCheckerRunning = false;
            });
        }

        /// <summary>
        /// Tests the response from the endpoint
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsEndpointRespondingAsync()
        {
            // If the endpoint IP is invalid
            if (EndpointIP == IPAddress.None)
            {
                // Tries to get the IP again
                EndpointIP = GetIPAddress(Hostname);
            }

            Ping ping = new Ping();
            try
            {
                // Awaits a ping reply
                Debugger.Log(0, "user", "Pinging endpoint...\n");
                PingReply reply = await ping.SendPingAsync(EndpointIP, 5000);
                // If a reply is received and is successful
                if (reply.Status == IPStatus.Success)
                    // The endpoint is responding
                    return true;
            }
            catch { }
            // The endpoint is not responding
            return false;
        }

        #endregion
    }
}
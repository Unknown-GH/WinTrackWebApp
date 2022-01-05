using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Renci.SshNet;

namespace WinTrackWebApp.Models
{
    public class Arduino
    {
        [Key]
        public int Key { get; set; } = 1;
        public bool DemoData { get; set; } = false;
        public bool Track { get; set; } = false;
        private static SshClient _client;

        public bool GetConnection()
        {
            if (DemoData) return true;
            if (_client == null || !_client.IsConnected) { 
                AuthenticationMethod method = new PasswordAuthenticationMethod("a3sec", "A3secwintrack6");
                ConnectionInfo connection = new ConnectionInfo("10.0.1.2", "a3sec", method); //TODO: FIX LATER
                connection.Timeout = TimeSpan.FromSeconds(1);
                _client = new SshClient(connection);

                try
                {
                    _client.Connect();
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    return false;
                }
                catch (Renci.SshNet.Common.SshOperationTimeoutException)
                {
                    return false;
                }
                Track = GetTrackStatus();
            }
            return true;
        }

        public void SwitchTrack()
        {
            if (DemoData) Track = !Track;
            else
            {
                if (GetConnection())
                {
                    _client.RunCommand("echo 2 >/dev/ttyACM0");
                    Track = !Track;
                }
            }
        }

        public bool GetTrackStatus()
        {
            if (DemoData) return Track;
            if (GetConnection())
            {
                var output = _client.RunCommand("echo 3 >/dev/ttyACM0");
                return Convert.ToBoolean(output.Result); //TODO: FIX LATER
            }
            return Track;
        }
    }
}

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
        public bool DemoSwitch { get; set; } = false;
        private static SshClient _client;

        public static bool GetConnection()
        {
            if (_client == null || !_client.IsConnected) { 
                AuthenticationMethod method = new PasswordAuthenticationMethod("a3sec", "A3secwintrack6");
                ConnectionInfo connection = new ConnectionInfo("10.0.1.2", "a3sec", method); //TODO: FIX LATER
                _client = new SshClient(connection);

                try
                {
                    _client.Connect();
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    return false;
                }
            }
            return true;
        }

        public void SwitchTrack()
        {
            if (DemoData) DemoSwitch = !DemoSwitch;
            else
            {
                if (GetConnection())
                {
                    _client.RunCommand("echo 2 >/dev/ttyACM0");
                }
            }
        }

        public bool GetStatus()
        {
            if (DemoData) return DemoSwitch;
            if (GetConnection())
            {
                var output = _client.RunCommand("echo 3 >/dev/ttyACM0");
                return Convert.ToBoolean(output.Result); //TODO: FIX LATER
            }
            return false;
        }

        /*public static string testConnection()
        {
            AuthenticationMethod method = new PasswordAuthenticationMethod("a3sec", "A3secwintrack6");
            ConnectionInfo connection = new ConnectionInfo("10.0.1.2", "a3sec", method);
            var client = new SshClient(connection);
            string output = "";

            try
            {
                client.Connect();
            }
            catch(System.Net.Sockets.SocketException e)
            {
                output += "ERROR: " + e.Message;
            }

            if (client.IsConnected)
            {
                output += "\nClient is connected to server";
                var readCommand = client.RunCommand("echo 2 >/dev/ttyACM0");
                output += readCommand.Result;
            }
            else
            {
                output += "\nClient could not connect to server";
            }

            return output;
        }*/
    }
}

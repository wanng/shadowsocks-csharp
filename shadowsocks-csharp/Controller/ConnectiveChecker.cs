using Shadowsocks.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Text;

namespace Shadowsocks.Controller
{
    class ConnectiveChecker
    {
        private Ping _ping;

        public ConnectiveChecker()
        {
            _ping = new Ping();
        }

        public int Ping(BindingList<Server> servers)
        {

            int timeout = 1000;
            long min = timeout;
            int i = 0;
            int minIndex = 0;

            foreach (Server server in servers)
            {
                try
                {
                    server.delay = I18N.GetString("Wait");

                    PingReply reply = _ping.Send(server.server, timeout);

                    if (IPStatus.Success.Equals(reply.Status))
                    {
                        server.delay = reply.RoundtripTime.ToString();

                        if (reply.RoundtripTime <= min)
                        {
                            min = reply.RoundtripTime;
                            minIndex = i;
                        }
                    }
                    else
                    {
                        server.delay = reply.Status.ToString();
                    }

                    i++;
                }
                catch (Exception)
                {
                    server.delay = I18N.GetString("Error");
                }
            }

            return minIndex;
        }
    }
}

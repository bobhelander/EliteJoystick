using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels.Ipc;
using System.Text;
using System.Threading.Tasks;

namespace EliteJoystick
{
    public class IpcServer
    {
        public void Start()
        {
            // Create the server channel.
            IpcChannel serverChannel = new IpcChannel("localhost:9090");

            // Register the server channel.
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(serverChannel, false);

            // Show the name of the channel.
            Console.WriteLine("The name of the channel is {0}.", serverChannel.ChannelName);

            // Show the priority of the channel.
            Console.WriteLine("The priority of the channel is {0}.",
                serverChannel.ChannelPriority);

            // Show the URIs associated with the channel.
            System.Runtime.Remoting.Channels.ChannelDataStore channelData =
                (System.Runtime.Remoting.Channels.ChannelDataStore)serverChannel.ChannelData;

            foreach (string uri in channelData.ChannelUris)
            {
                Console.WriteLine("The channel URI is {0}.", uri);
            }

            // Expose an object for remote calls.
            System.Runtime.Remoting.RemotingConfiguration.RegisterWellKnownServiceType(
                    typeof(Communication.EDDIStatus), "EDDIStatus.rem",
                    System.Runtime.Remoting.WellKnownObjectMode.Singleton);

            // Parse the channel's URI.
            string[] urls = serverChannel.GetUrlsForUri("EDDIStatus.rem");
            if (urls.Length > 0)
            {
                string objectUrl = urls[0];
                string objectUri;
                string channelUri = serverChannel.Parse(objectUrl, out objectUri);
                Console.WriteLine("The object URI is {0}.", objectUri);
                Console.WriteLine("The channel URI is {0}.", channelUri);
                Console.WriteLine("The object URL is {0}.", objectUrl);
            }
        }
    }
}

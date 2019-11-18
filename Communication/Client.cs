using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCommunication
{
    public class Client
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        NamedPipeClientStream client = null;
        StreamWriter stream = null;

        public async Task CreateConnection(string pipeName)
        {
            log.Debug($"Attempting to connect to: {pipeName}");
            try
            {
                client = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.Asynchronous);
                //await client.ConnectAsync(30000);
                await client.ConnectAsync(30000).ConfigureAwait(false);
                stream = new StreamWriter(client);
                log.Debug($"Connection established to: {pipeName}");
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        public bool IsConnected()
        {
            if (client != null)
                return client.IsConnected;
            return false;
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                // write the message to the pipe stream 
                await stream.WriteLineAsync(message).ConfigureAwait(false);
                await stream.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
        }

        public void SendMessage(string message)
        {
            Task.Run(() => SendMessageAsync(message));
        }
    }
}

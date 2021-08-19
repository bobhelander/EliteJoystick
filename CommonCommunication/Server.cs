using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCommunication
{
    public class Server
    {
        public bool ContinueListening { get; set; }
        public ILogger Logger { get; set; }

        public async Task StartListening(string pipeName, Action<string> messageRecieved)
        {
            try
            {
                while (ContinueListening)
                {
                    using (var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                    {
                        Logger?.LogDebug($"Listening on {pipeName}");

                        // wait for the connection
                        pipe.WaitForConnectionAsync().Wait();

                        using (var streamReader = new StreamReader(pipe))
                        {
                            while (ContinueListening)
                            {
                                // read the message from the stream - async
                                var message = await streamReader.ReadLineAsync().ConfigureAwait(false);

                                // invoke the message received action
                                var task = Task.Run(() => messageRecieved?.Invoke(message))
                                    .ContinueWith(t => { Logger?.LogError($"Message Received Exception: {t.Exception}"); }, TaskContinuationOptions.OnlyOnFaulted);

                                if (streamReader.EndOfStream)
                                {
                                    Logger?.LogInformation("End Of Stream");
                                    break;
                                }
                            }
                        }
                        if (pipe.IsConnected)
                        {
                            // must disconnect 
                            pipe.Disconnect();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Logger?.LogError(exception.Message);
            }
        }
    }
}

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
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool ContinueListening { get; set; }

        public async Task StartListening(string pipeName, Action<string> messageRecieved)
        {
            try
            {
                while (ContinueListening)
                {
                    using (var pipe = new NamedPipeServerStream(pipeName, PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                    {
                        log.Debug($"Listening on {pipeName}");

                        // wait for the connection
                        pipe.WaitForConnectionAsync().Wait();

                        using (var streamReader = new StreamReader(pipe))
                        {
                            while (ContinueListening)
                            {
                                // read the message from the stream - async
                                var message = await streamReader.ReadLineAsync();

                                // invoke the message received action
                                Task.Factory.StartNew(() => messageRecieved?.Invoke(message));

                                if (streamReader.EndOfStream)
                                {
                                    log.Info("End Of Stream");
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
                log.Error(exception);
            }
        }
    }
}

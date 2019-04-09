using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Threading;
using WebSocket4Net;
using Newtonsoft.Json;

namespace ChromeController
{
    // https://github.com/martinkunc/automate-chrome
    // https://chromedevtools.github.io/devtools-protocol/

    public class Chrome
    {
        public class Parameters
        {
            public string expression { get; set; } = "Runtime.evaluate";
            public string objectGroup { get; set; } = "console";
            public bool includeCommandLineAPI { get; set; } = true;
            public bool doNotPauseOnExceptions { get; set; } = false;
            public bool returnByValue { get; set; } = false;
        };

        public class Message
        {
            public string method { get; set; } = "Runtime.evaluate";
            [JsonProperty(PropertyName = "params")]
            public Parameters Params { get; set; } = new Parameters();
            public int id { get; set; } = 1;
        }


        const string JsonPostfix = "/json";

        string remoteDebuggingUri;

        public ChromeSession CurrentSession { get; set; }

        public Chrome(string remoteDebuggingUri) {
            this.remoteDebuggingUri = remoteDebuggingUri;
        }

        public TRes SendRequest<TRes>()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(remoteDebuggingUri + JsonPostfix);
            var resp = req.GetResponse();
            var respStream = resp.GetResponseStream();

            StreamReader sr = new StreamReader(respStream);
            var s = sr.ReadToEnd();
            resp.Dispose();
            return JsonConvert.DeserializeObject<TRes>(s);
        }

        public List<ChromeSession> GetAvailableSessions()
        {
            try
            {
                var res = this.SendRequest<List<ChromeSession>>();
                return (from r in res
                        where r.devtoolsFrontendUrl != null
                        select r).ToList();
            }
            catch(Exception ex)
            {
                var test= "test";
            }
            return null;
        }

        public List<ChromeSession> GetAvailablePageSessions()
        {
            try
            {
                var res = this.SendRequest<List<ChromeSession>>();
                return (from r in res
                        where r.devtoolsFrontendUrl != null && r.type == "page"
                        select r).ToList();
            }
            catch (Exception ex)
            {
                var test = "test";
            }
            return null;
        }

        public ChromeSession OpenNewTab(string uri)
        {
            var requestUrl = $"{remoteDebuggingUri}/json/new?{uri}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            var resp = req.GetResponse();
            var respStream = resp.GetResponseStream();

            StreamReader sr = new StreamReader(respStream);
            var s = sr.ReadToEnd();
            resp.Dispose();

            CurrentSession = JsonConvert.DeserializeObject<ChromeSession>(s);
            return CurrentSession;
        }

        public string Scroll(ChromeSession session, int distance)
        {
            // window.scrollTo(0,0)
            // window.scrollBy(0,0)
            // window.pageYOffset
            // window.pageXOffset

            var message = new Message();
            message.Params.expression = $"window.scrollBy(0, {distance})";

            var json = JsonConvert.SerializeObject(message);
            return this.SendCommand(session, json);
        }

        public void ActivateTab(ChromeSession session)
        {
            var requestUrl = $"{remoteDebuggingUri}/json/activate/{session.id}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            var resp = req.GetResponse();

            CurrentSession = session;
        }

        public void CloseTab(ChromeSession session)
        {
            var requestUrl = $"{remoteDebuggingUri}/json/close/{session.id}";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            var resp = req.GetResponse();

            CurrentSession = null;
        }

        public string NavigateTo(ChromeSession session, string uri)
        {
            // Page.navigate is working from M18
            //var json = @"{""method"":""Page.navigate"",""params"":{""url"":""" + uri + @"""},""id"":1}";

            // Instead of Page.navigate, we can use document.location
            var message = new Message();
            message.Params.expression = $"document.location.href='{uri}'";

            var json = JsonConvert.SerializeObject(message);
            return this.SendCommand(session, json);
        }

        public void ChangeTab(int number)
        {
            var sessions = GetAvailablePageSessions();
            if (sessions.Count > 0 && null != CurrentSession)
            {
                int currentIndex = sessions.FindIndex(x => x.id == CurrentSession.id);

                currentIndex += number;

                if (currentIndex < 0)
                {
                    currentIndex = 0;
                }
                if (currentIndex >= sessions.Count)
                {
                    currentIndex = sessions.Count - 1;
                }

                ActivateTab(sessions[currentIndex]);
            }
        }

        public string NavigateTo2(ChromeSession session, string uri)
        {
            // Page.navigate is working from M18
            //var json = @"{""method"":""Page.navigate"",""params"":{""url"":""" + uri + @"""},""id"":1}";

            // Instead of Page.navigate, we can use document.location
            var json = @"{""method"":""Runtime.evaluate"",""params"":{""expression"":""document.location.href='"+uri+@"'"",""objectGroup"":""console"",""includeCommandLineAPI"":true,""doNotPauseOnExceptions"":false,""returnByValue"":false},""id"":1}";
            return this.SendCommand(session, json);
        }

        public string GetElementsByTagName(ChromeSession session, string tagName)
        {
            // Page.navigate is working from M18
            //var json = @"{""method"":""Page.navigate"",""params"":{""url"":""http://www.seznam.cz""},""id"":1}";

            // Instead of Page.navigate, we can use document.location
            var json = @"{""method"":""Runtime.evaluate"",""params"":{""expression"":""document.getElementsByTagName('" + tagName+ @"')"",""objectGroup"":""console"",""includeCommandLineAPI"":true,""doNotPauseOnExceptions"":false,""returnByValue"":false},""id"":1}";
            return this.SendCommand(session, json);
        }


        public string Eval(ChromeSession session, string cmd)
        {
            var json = @"{""method"":""Runtime.evaluate"",""params"":{""expression"":"""+cmd+@""",""objectGroup"":""console"",""includeCommandLineAPI"":true,""doNotPauseOnExceptions"":false,""returnByValue"":false},""id"":1}";
            return this.SendCommand(session, json);
        }

        public string SendCommand(ChromeSession session, string cmd)
        {
            return SendCommand(UpdateEndpoint(session.webSocketDebuggerUrl), cmd);
        }

        private string SendCommand(string sessionWSEndpoint, string cmd)
        {
            WebSocket4Net.WebSocket j = new WebSocket4Net.WebSocket(sessionWSEndpoint);
            ManualResetEvent waitEvent = new ManualResetEvent(false);
            ManualResetEvent closedEvent = new ManualResetEvent(false);
            string message = "";
            byte[] data;

            Exception exc = null;
            j.Opened += delegate(System.Object o, EventArgs e) {
                j.Send(cmd);
            };

            j.MessageReceived += delegate(System.Object o, MessageReceivedEventArgs e) {
                message = e.Message;
                waitEvent.Set();
            };

            j.Error += delegate(System.Object o, SuperSocket.ClientEngine.ErrorEventArgs e)
            {
                exc = e.Exception;
                waitEvent.Set();
            };

            j.Closed += delegate(System.Object o, EventArgs e)
            {
                closedEvent.Set();
            };

            j.DataReceived += delegate(System.Object o, DataReceivedEventArgs e)
            {
                data = e.Data;
                waitEvent.Set();
            };

            j.Open();
            
            waitEvent.WaitOne();
            if (j.State == WebSocket4Net.WebSocketState.Open)
            {
                j.Close();
                closedEvent.WaitOne();
            }
            if (exc != null)
                throw exc;
            
            return message;
        }

        private string UpdateEndpoint(string sessionWSEndpoint)
        {
            // Sometimes binding to localhost might resolve wrong AddressFamily, force IPv4
            return sessionWSEndpoint.Replace("ws://localhost", "ws://127.0.0.1");
        }
    }
}

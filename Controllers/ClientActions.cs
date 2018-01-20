using CommonCommunication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public static class ClientActions
    {
        public class ClientEventArgs : EventArgs
        {
            public String Message { get; set; }
        }

        public static event EventHandler<ClientEventArgs> ClientAction;

        public static void Action(object sender, String message)
        {
            ClientAction(sender, new ClientEventArgs { Message = message });
        }

        public static string ClipboardAction()
        {
            return JsonConvert.SerializeObject(new Message { Type = "clipboard", Data = "" });
        }

        public static void ClientInformationAction(object sender, string informationMessage)
        {
            Action(sender, GetMessage("client_information", informationMessage));
        }

        public static void KillProcess(object sender)
        {
            Action(sender, GetMessage("kill", String.Empty));
        }

        public static void FocusProcess(object sender)
        {
            Action(sender, GetMessage("focus", String.Empty));
        }

        public static string GetMessage(string messageType, string message)
        {
            return JsonConvert.SerializeObject(new Message { Type = messageType, Data = message });
        }
    }
}

using PrimitiveChatBot.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PrimitiveChatBot.Common
{   

    public class ViewHandler
    {
        public static Frame MainFrame { get; set; }


        private static Builder _builder = new Builder();

        private static Chatbot _chatbot = new Chatbot();

        public static void NavigateTo(string page = "DEFAULT")
        {
            switch (page)
            {
                case "Builder":
                    MainFrame.Navigate(_builder);
                    break;
                default:
                    MainFrame.Navigate(_chatbot);
                    break;
            }
        }

    }
}

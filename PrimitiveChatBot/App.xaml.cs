using PrimitiveChatBot.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace PrimitiveChatBot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static bool IsAdmin { get; private set; } = false;

        /// <summary>
        /// BotEngine for the application to run on
        /// 
        /// Core interaction interface and the base for this application
        /// </summary>
        public static BotEngine BotEngine { get; private set; }

        public App()
        {
            // Initialize the Botengine when the App is initialized
            BotEngine = new BotEngine();
        }

        /// <summary>
        /// Apply the Admin mode as soon as the application starts and the parameter is set
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                foreach (string arg in e.Args)
                {
                    if (arg.ToLower() == "--admin")
                    {
                        IsAdmin = true;
                        break;
                    }
                }
            }
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream? stream = assembly.GetManifestResourceStream("PrimitiveChatBot.messages.json"))
            {
                if (stream != null)
                {
                    BotEngine.Storage.Import(stream);
                }
            }
        }
    }
}

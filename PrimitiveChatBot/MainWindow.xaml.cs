using PrimitiveChatBot.Common;
using PrimitiveChatBot.Controls;
using System.Windows;
using System.Windows.Input;

namespace PrimitiveChatBot
{
    /// <summary>
    /// Code-Behind for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Default Ctor called by WPF on page load
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            ViewHandler.MainFrame = MainFrame;
            // Navigate to the default page on initialization
            ViewHandler.NavigateTo();
        }
    }
}

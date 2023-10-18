using Microsoft.Win32;
using PrimitiveChatBot.Common;
using StorageLib.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimitiveChatBot.Pages
{
    /// <summary>
    /// Code-Behind for Builder.xaml
    /// </summary>
    public partial class Builder : Page
    {
        /// <summary>
        /// Default Ctor called by WPF on page load
        /// </summary>
        public Builder()
        {
            InitializeComponent();
            DataContext = App.BotEngine.Storage;
        }

        /// <summary>
        /// Handle the BtnBack click event and Navigate back to the default Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            ViewHandler.NavigateTo();
        }

 
        private async void BtnImportCSV_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "JSON Dateien (*.json)|*.json";

                if (openFileDialog.ShowDialog() == true)
                {
                    var file = openFileDialog.FileName;
                    
                    await Task.Run(() =>
                    {
                        try
                        {
                            App.BotEngine.Storage.Import(file);
                        }
                        catch (ImportFormatException ex)
                        {
                            MessageBox.Show(ex.Text, ex.Caption, MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        catch (InvalidDataException)
                        {
                            MessageBox.Show("Die Import datei muss gültiges JSON enthalten.", "Dateiformat Ungültig", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    });
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unerwarteter Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

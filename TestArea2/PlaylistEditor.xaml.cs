using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data;
using System.Windows.Forms;

namespace TestArea2
{
    /// <summary>
    /// Interaction logic for PlaylistEditor.xaml
    /// </summary>
    public partial class PlaylistEditor : Window
    {
        private string newFilePath;
        public PlaylistEditor()
        {
            InitializeComponent();
            DataView dataView = GlobalVars.SongsInCurrentPl.DefaultView;
            LstBox_PslEditor.ItemsSource = dataView;
            

        }

        private void GMBPlusButtonClicked(object sender, RoutedEventArgs e)
        {

            AddSongWindow newSongWindow = new AddSongWindow();
            newSongWindow.Show();
            
        }
    }
}

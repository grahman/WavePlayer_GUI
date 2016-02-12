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
using System.Windows.Forms;

namespace TestArea2
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddSongWindow : Window
    {
        public delegate void LibraryContentChangedHandler(object sender, GMBEventArgs e);
        public event LibraryContentChangedHandler LibraryContentChanged;
        public AddSongWindow()
        {
            InitializeComponent();
        }

        private void GMBBrowseButtonClicked(object sender, RoutedEventArgs e)
        {
            AddSongArgs songArgs = new AddSongArgs();
            GlobalVars.newSongArgs = songArgs;

            string newFilePath = "";

            OpenFileDialog waveFilechooser = new OpenFileDialog();
            if (waveFilechooser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TxtBox_Path.Text = waveFilechooser.FileName;
                songArgs.path = waveFilechooser.FileName;

            }
            else
            {
                
            }


            
        }

        private void GMBSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            Console.Write("breakpoint");
            GlobalVars.newSongArgs.name = TxtBox_SongName.Text;
            GlobalVars.newSongArgs.path = TxtBox_Path.Text;
            try
            {
                GlobalVars.TblAdp_Files.Insert(GlobalVars.newSongArgs.name, GlobalVars.newSongArgs.path, 44, 10000);
                LibraryContentChanged(this, null);
                this.Close();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("There was an error adding the song to the database: " + ex.Message);
            }

        }
    }
}

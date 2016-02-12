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

namespace TestArea2
{
    /// <summary>
    /// Interaction logic for ManageLibrary.xaml
    /// </summary>
    public partial class ManageLibrary : Window
    {
        public delegate void LibraryContentChangedHandler(object sender, GMBEventArgs e);
        public delegate void PlaylistContentChangedHandler(object sender, GMBEventArgs e);
        public event LibraryContentChangedHandler LibraryContentChanged;
        public event PlaylistContentChangedHandler PlaylistContentChanged;
        private GMBAudioLibDataSetTableAdapters.FILESTableAdapter TblAdp_Files;
        private GMBAudioLibDataSet.FILESDataTable DtaTbl_Files;
        public ManageLibrary()
        {
            InitializeComponent();
            ManageLibrary_PlaylistContentChanged(null, null); 
            DataView plDataView;
            if (GlobalVars.SongsInCurrentPl != null)
            {
                plDataView = GlobalVars.SongsInCurrentPl.DefaultView;
            }
            else
            {
                plDataView = new DataView();
            }
            
            LstBox_Pl.ItemsSource = plDataView;
            TblAdp_Files = new GMBAudioLibDataSetTableAdapters.FILESTableAdapter();
            DtaTbl_Files = TblAdp_Files.GetData();

            DataView libDataView = DtaTbl_Files.DefaultView;
            LstBox_LibView.ItemsSource = libDataView;

            LibraryContentChanged += GMBLibraryContentChangedEventHandler;
            PlaylistContentChanged += ManageLibrary_PlaylistContentChanged;
        }

        void ManageLibrary_PlaylistContentChanged(object sender, GMBEventArgs e)
        {
            GlobalVars.TblAdp_Pl = new GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter();
            GlobalVars.DtaTbl_Pl = GlobalVars.TblAdp_Pl.GetData();
            GlobalVars.TblAdp_Pl.Fill(GlobalVars.DtaTbl_Pl);
            DataView dataView = GlobalVars.DtaTbl_Pl.DefaultView;
            GMBAudioLibDataSetTableAdapters.SongsInPlaylistTableAdapter TblAdp_SongsInPl = new GMBAudioLibDataSetTableAdapters.SongsInPlaylistTableAdapter();
            GMBAudioLibDataSet.SongsInPlaylistDataTable SongsInCurrentPl = TblAdp_SongsInPl.GetSongsInPlaylist(GlobalVars.CurrentPlaylist);

            GlobalVars.SongsInCurrentPl = SongsInCurrentPl;
            GlobalVars.TblAdp_SongsInPl = TblAdp_SongsInPl;
            LstBox_Pl.ItemsSource = null;
            LstBox_Pl.ItemsSource = dataView;
        }

        private void GMBPlusButtonClicked(object sender, RoutedEventArgs e)
        {
            AddSongWindow Wdw_AddSong = new AddSongWindow();
            Wdw_AddSong.LibraryContentChanged += GMBLibraryContentChangedEventHandler;
            Wdw_AddSong.Show();
        }

        private void GMBLibraryContentChangedEventHandler(object sender, GMBEventArgs e)
        {
            TblAdp_Files = new GMBAudioLibDataSetTableAdapters.FILESTableAdapter();
            DtaTbl_Files = TblAdp_Files.GetData();
            DataView dataView = DtaTbl_Files.DefaultView;
           
            LstBox_LibView.ItemsSource = dataView;
            //DtaTbl_Files = 
        }

        private void GMBMinusButtonClicked(object sender, RoutedEventArgs e)
        {
            //Get the selected item.
            int selectedIndex = LstBox_LibView.SelectedIndex;
            if (LstBox_LibView.SelectedItem != null)
            {
                System.Data.DataRowView selectedDataRowView = (System.Data.DataRowView)LstBox_LibView.Items[selectedIndex];
                string name = selectedDataRowView.Row[0].ToString();
                try
                {
                    GlobalVars.TblAdp_Files.Delete1(name);
                    LibraryContentChanged(this, null);
                }
                catch (Exception ex)
                {
                    //System.Windows.MessageBox.Show("Couldn't delete song from library: " + ex.Message);
                    MessageBox.Show("Cannot delete song from library if it is currently used in a playlist.");
                }

                Console.Write("breakpoint");
            }

        }

        private void GMBAddToPlaylistButtonClicked(object sender, RoutedEventArgs e)
        {
            int selectedIndex = LstBox_LibView.SelectedIndex;
            int plCount = 0;
            //GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter plContext = new GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter();
            GlobalVars.TblAdp_Pl = new GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter();
            GlobalVars.TblAdp_NumberOfSongsInPlaylist = new GMBAudioLibDataSetTableAdapters.PLAYLISTCOUNTTableAdapter();
            if (GlobalVars.CurrentPlaylist == null)
            {
                GlobalVars.CurrentPlaylist = "Test";
            }
            GlobalVars.DtaTbl_NumberOfSongsInPlaylist = GlobalVars.TblAdp_NumberOfSongsInPlaylist.GetPlayListCount(GlobalVars.CurrentPlaylist);
            
            if (LstBox_LibView.SelectedItem != null)
            {
                
                System.Data.DataRowView selectedDataRowView = (System.Data.DataRowView)LstBox_LibView.Items[selectedIndex];
                string name = selectedDataRowView.Row[0].ToString();
                try
                {
                    plCount = int.Parse(GlobalVars.DtaTbl_NumberOfSongsInPlaylist[0].NUM.ToString());
                    //plCount = GlobalVars.DtaTbl_NumberOfSongsInPlaylist.Count;
                    GlobalVars.TblAdp_Pl.Insert(GlobalVars.CurrentPlaylist, plCount, name);
                    PlaylistContentChanged(this, null);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Couldn't add song to playlist: " + ex.Message);
                }

                Console.Write("breakpoint");
            }
        }

        private void GMBDeleteFromPlaylistButtonClicked(object sender, RoutedEventArgs e)
        {
            //Get the selected item.
            GlobalVars.TblAdp_Pl = new GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter();
            int selectedIndex = LstBox_Pl.SelectedIndex;
            int plIndex;
            if (LstBox_Pl.SelectedItem != null)
            {
                System.Data.DataRowView selectedDataRowView = (System.Data.DataRowView)LstBox_Pl.Items[selectedIndex];
                string name = selectedDataRowView.Row[1].ToString();
                try
                {
                    //GlobalVars.TblAdp_Pl.Delete(GlobalVars.CurrentPlaylist, selectedIndex, name);
                   //GlobalVars.TblAdp_Pl.GMBDelete(GlobalVars.CurrentPlaylist, selectedIndex);
                   // PlaylistContentChanged(this, null);

                   DeletePlaylistItem(GlobalVars.CurrentPlaylist, selectedIndex);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Couldn't delete song from playlist: " + ex.Message);
                }

                Console.Write("breakpoint");
            }
        }

        private void DeletePlaylistItem(string _plname, int _currentIndex)
        {
            int nSongsInPl;
            //Delete the item
            try
            {
                //GlobalVars.TblAdp_Pl.Delete(GlobalVars.CurrentPlaylist, selectedIndex, name);
                GlobalVars.TblAdp_Pl.GMBDelete(_plname, _currentIndex);
               

                //Update the indexes of all the songs with indexes > _currentIndex
                if (GlobalVars.TblAdp_NumberOfSongsInPlaylist == null)
                {
                    GlobalVars.TblAdp_NumberOfSongsInPlaylist = new GMBAudioLibDataSetTableAdapters.PLAYLISTCOUNTTableAdapter();
                }
                nSongsInPl = int.Parse(GlobalVars.TblAdp_NumberOfSongsInPlaylist.GetPlayListCount(_plname)[0].NUM.ToString());
                while (_currentIndex <= nSongsInPl)
                {
                    ++_currentIndex;
                    GlobalVars.TblAdp_Pl.GMBUpdate(_currentIndex - 1, _plname, _currentIndex);
                }
                   
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Couldn't delete song from playlist: " + ex.Message);
            }

            PlaylistContentChanged(this, null);
            

        }

        private void GMBMoveUpPlaylistButtonClicked(object sender, RoutedEventArgs e)
        {
            string cp = GlobalVars.CurrentPlaylist;

            //Get index of selected song.

            int selectedIndex = LstBox_Pl.SelectedIndex;
            int originalIndex = selectedIndex - 1;

            if (selectedIndex < 1)
                return;

            //Get max number of songs in playlist
            int plCount = int.Parse(GlobalVars.DtaTbl_NumberOfSongsInPlaylist[0].NUM.ToString());

            //Set the current song to a higher, unused index.
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, plCount + 1, cp, selectedIndex);
            }
            catch 
            {
                MessageBox.Show("There was an error updating the index of the playlist item...");
            }

            //Now set the previous song to the higher index
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, selectedIndex, cp, originalIndex);
            }
            catch
            {
                MessageBox.Show("There was an error incrementing the index of the playlist item...");
            }

            //Now put the original song we are trying to move back to the "original index"
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, originalIndex, cp, plCount + 1);
            }
            catch
            {
                MessageBox.Show("There was an error in the final move operation...");
            }

            PlaylistContentChanged(this, null);
            LstBox_Pl.SelectedIndex = originalIndex;
            
        }

        private void GMBMoveDownPlaylistButtonClicked(object sender, RoutedEventArgs e)
        {
            string cp = GlobalVars.CurrentPlaylist;

            //Get max number of songs in playlist
            int plCount = int.Parse(GlobalVars.DtaTbl_NumberOfSongsInPlaylist[0].NUM.ToString());

            //Get index of selected song.
            int selectedIndex = LstBox_Pl.SelectedIndex;
            int originalIndex = selectedIndex + 1;

            if (selectedIndex + 1 >= plCount)
                return;


            //Set the current song to a higher, unused index.
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, plCount + 1, cp, selectedIndex);
            }
            catch
            {
                MessageBox.Show("There was an error updating the index of the playlist item...");
            }

            //Now set the previous song to the higher index
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, selectedIndex, cp, originalIndex);
            }
            catch
            {
                MessageBox.Show("There was an error incrementing the index of the playlist item...");
            }

            //Now put the original song we are trying to move back to the "original index"
            try
            {
                GlobalVars.TblAdp_Pl.UpdateIndex(cp, originalIndex, cp, plCount + 1);
            }
            catch
            {
                MessageBox.Show("There was an error in the final move operation...");
            }

            PlaylistContentChanged(this, null);
            LstBox_Pl.SelectedIndex = originalIndex;
        }
    }
}

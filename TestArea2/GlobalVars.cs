using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestArea2
{

    public struct AddSongArgs
    {
        public string name;
        public string path;
    }
    public class GlobalVars
    {
        private static string currentPl;
        public static string CurrentPlaylist { get { return currentPl;} set { currentPl = value;}}

        public static GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter TblAdp_Pl;
        public static GMBAudioLibDataSetTableAdapters.FILESTableAdapter TblAdp_Files;
        public static GMBAudioLibDataSetTableAdapters.PLAYLISTNAMESTableAdapter TblAdp_PlNames;
        public static GMBAudioLibDataSetTableAdapters.SongsInPlaylistTableAdapter TblAdp_SongsInPl;
        public static GMBAudioLibDataSetTableAdapters.PLAYLISTCOUNTTableAdapter TblAdp_NumberOfSongsInPlaylist;
             
        public static GMBAudioLibDataSet.PLAYLISTSDataTable DtaTbl_Pl;
        public static GMBAudioLibDataSet.FILESDataTable DtaTbl_Files;
        public static GMBAudioLibDataSet.SongsInPlaylistDataTable SongsInCurrentPl;
        public static GMBAudioLibDataSet.PLAYLISTCOUNTDataTable DtaTbl_NumberOfSongsInPlaylist;

        public static GMBAudioLibDataSet.PLAYLISTNAMESDataTable PlNames;

        public static AddSongArgs newSongArgs;

       
        
    }


}

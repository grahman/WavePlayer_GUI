using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Data.Entity;
using ATLProject2Lib;
using System.Windows.Forms;
using System.Timers;
using System.Runtime.InteropServices;

namespace TestArea2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
  
    public partial class MainWindow : Window
    {
        public delegate void fileOpenDelegate(object sender, EventArgs e);
        public delegate void InterOpEventWavePositionChangedEventHandler(ulong newPos);
        public IntPtr fp;
        public InterOpEventWavePositionChangedEventHandler GMBOnPosLChangedPtr;        //A function pointer.
        public fileOpenDelegate Handler;
        public string currentPl;
        public GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter TblAdp_Pl;
        public GMBAudioLibDataSetTableAdapters.FILESTableAdapter TblAdp_Files;
        public GMBAudioLibDataSetTableAdapters.PLAYLISTNAMESTableAdapter TblAdp_PlNames;
        public GMBAudioLibDataSetTableAdapters.SongsInPlaylistTableAdapter TblAdp_SongsInPl;
        public GMBAudioLibDataSetTableAdapters.PLAYLISTCOUNTTableAdapter TblAdp_PlCount;

        public GMBAudioLibDataSet.PLAYLISTSDataTable DtaTbl_Pl;
        public GMBAudioLibDataSet.FILESDataTable DtaTbl_Files;
        public GMBAudioLibDataSet.SongsInPlaylistDataTable SongsInCurrentPl;

        public GMBAudioLibDataSet.PLAYLISTNAMESDataTable PlNames;

        public string selectedFilepathL;
        public string selectedFilepathR;

        public bool isPlaying;
        public bool userChangingFilter;

        private GMBSimpleObj audioPlayer;

        private float filter_cu;
        private float filter_q;
        private GMBSoundPlayer AUWrapper;
        public float FilterCutoffFrequency 
        {
            get
            {
                return filter_cu;
            } 
            set 
            {
                filter_cu = value;
                if (audioPlayer == null)
                    return;
                audioPlayer.FilterCutoff = filter_cu;
            }
        }
        public float FilterQ 
        {
            get {return filter_q;} 
            set 
            { 
                filter_q = value;
                if (audioPlayer == null)
                    return;
                audioPlayer.FilterQ = filter_q;
            }
        }

        //State variables
        private bool xyPadInUse;

        //Other utils
        private System.Timers.Timer timer;
        private System.Timers.Timer timerR;
        private bool firstRunComplete;

        public MainWindow()
        {
            InitializeComponent();
            
            userChangingFilter = false;
            //CurrentPlaylist = "test";
            TblAdp_Pl = new GMBAudioLibDataSetTableAdapters.PLAYLISTSTableAdapter();
            TblAdp_Files = new GMBAudioLibDataSetTableAdapters.FILESTableAdapter();
            TblAdp_PlNames = new GMBAudioLibDataSetTableAdapters.PLAYLISTNAMESTableAdapter();
            TblAdp_PlCount = new GMBAudioLibDataSetTableAdapters.PLAYLISTCOUNTTableAdapter();



            DtaTbl_Files = TblAdp_Files.GetData();
            DtaTbl_Pl = TblAdp_Pl.GetData();
            

            //SongsInCurrentPl = 

            PlNames = TblAdp_PlNames.GetDistinctPlaylistNames();
           
           if (PlNames.Count > 0)
           {
               currentPl = PlNames[0].PLNAME.ToString();
           }
            

            //TblAdp_SongsInPl = new GMBAudioLibDataSetTableAdapters.SongsInPlaylistTableAdapter();
            //SongsInCurrentPl = TblAdp_SongsInPl.GetSongsInPlaylist(currentPl);

            //Now put these instance variables into our Global Variables.

            GlobalVars.DtaTbl_Pl    = DtaTbl_Pl;
            GlobalVars.DtaTbl_Files = DtaTbl_Files;
            GlobalVars.CurrentPlaylist = currentPl;
            GlobalVars.PlNames = PlNames;
            GlobalVars.SongsInCurrentPl = SongsInCurrentPl;
            GlobalVars.TblAdp_Files = TblAdp_Files;
            GlobalVars.TblAdp_PlNames = TblAdp_PlNames;
            GlobalVars.TblAdp_SongsInPl = TblAdp_SongsInPl;
            GlobalVars.DtaTbl_NumberOfSongsInPlaylist = TblAdp_PlCount.GetPlayListCount(currentPl);

            audioPlayer = new GMBSimpleObj();
            audioPlayer.Init();

            //WaveformTimelineDisplayL = new WPFSoundVisualizationLib.WaveformTimeline();
            
            FilterCutoffFrequency = 19000.0F;
            FilterQ = 0.707F;

            //State variable initialization
            xyPadInUse = false;

            Console.Write("\n");
            Sldr_GainL.Value = 1.0;
            Sldr_GainR.Value = 1.0;
            Sldr_XFade.Value = 0.0;   
            AUWrapper = new GMBSoundPlayer(audioPlayer);
            timer = new System.Timers.Timer(1);
            
            timer.Elapsed += timer_Elapsed;
            timer.SynchronizingObject = wViewer;

            timerR = new System.Timers.Timer(10);
            timerR.Elapsed += timer_ElapsedR;
            timerR.SynchronizingObject = wViewerR;
            
            wViewer.SamplesPerPixel = 100;
            firstRunComplete = false;
            GMBOnPosLChangedPtr = new InterOpEventWavePositionChangedEventHandler(GMBOnPosLChanged);
            fp = Marshal.GetFunctionPointerForDelegate(GMBOnPosLChangedPtr);
            audioPlayer.PosChangedL_RegisterCallback(fp);
            string fp_loc = "fp memory locations is: " + fp.ToString();
            System.IO.File.WriteAllText(@"C:\\Users\\Graham\\Programming\\Logs\\interopEventsLogManaged.txt", fp_loc);
            //ATLProject2Lib.GMBSimpleObj so;
            //audioPlayer.OnPosChangedL += audioPlayer_OnPosChangedL;
            
            
            
            
        }

        void audioPlayer_OnPosChangedL(uint x)
        {
            //throw new NotImplementedException();
            wViewer.StartPosition = (int)x;
        }


        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ulong newPos = audioPlayer.getPosL();
            wViewer.StartPosition = (long)newPos * 2;
            wViewer.Refresh();
        }

        void timer_ElapsedR(object sender, ElapsedEventArgs e)
        {
            wViewerR.StartPosition += 2790;
            wViewerR.Refresh();
        }

        private void GMBInit(object sender, RoutedEventArgs e)
        {
            //WaveformTimelineDisplayL = new WPFSoundVisualizationLib.WaveformTimeline();
        }

        private void GMBWaveformTimelineClickedL(object sender, MouseButtonEventArgs e)
        {
            MarkerEditor markerEditorWindowL = new MarkerEditor();
            markerEditorWindowL.Show();
        }

        private void GMBPlaylistEditorMenuItemClicked(object sender, RoutedEventArgs e)
        {
            PlaylistEditor plEditor = new PlaylistEditor();
            plEditor.Show();
        }

        private void GMBManageLibraryMenuItemClicked(object sender, RoutedEventArgs e)
        {
            ManageLibrary libWindow = new ManageLibrary();
            libWindow.Show();
        }

        private void onLibraryChanged(object sender, GMBEventArgs e)
        {
            
        }

        private void GMBPlayButtonClicked(object sender, RoutedEventArgs e)
        {
            int sleepDuration = 1;
            if (!Convert.ToBoolean(audioPlayer.IsReadyL()))
            {
                return;
            }
            if (!isPlaying)
            {
                if (!firstRunComplete)
                {
                    firstRunComplete = true;
                    isPlaying = true;
                    audioPlayer.Run();
                    Thread.Sleep(sleepDuration);
                    timer.Start();
                }
                else
                {
                    isPlaying = true;
                    audioPlayer.Play_Pause();
                    //audioPlayer.Run();
                    Thread.Sleep(sleepDuration);
                    timer.Start();
                } 
            }
            else
            {
                //audioPlayer.ChangeState(0);
                audioPlayer.Play_Pause();
                //Thread.Sleep(sleepDuration);
                timer.Stop();
                isPlaying = false;
            }

          
        }

        private void GMBToggleTimer()
        {
            if (timer.Enabled == true)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void GMBLoadAudioButtonClickedL(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofdialog = new System.Windows.Forms.OpenFileDialog();
            ofdialog.Title = "Select a *.wav file";
            ofdialog.Multiselect = false;

            ofdialog.FileOk += ofdialog_FileOk;

          
            ofdialog.ShowDialog();    
        }

        private void GMBLoadAudioButtonClickedR(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog ofdialogR = new System.Windows.Forms.OpenFileDialog();
            ofdialogR.Title = "Select a *.wav file";
            ofdialogR.Multiselect = false;

            ofdialogR.FileOk += ofdialogR_FileOk;

           
            ofdialogR.ShowDialog();    
        }

        void ofdialogR_FileOk(object sender, CancelEventArgs e)
        {
            if (isPlaying)
            {
                GMBToggleTimer();
            }
            System.Windows.Forms.OpenFileDialog ofdialog = (System.Windows.Forms.OpenFileDialog)sender;
            BackgroundWorker bwR = new BackgroundWorker();
            bwR.DoWork += BGWorkerR;
            bwR.RunWorkerCompleted += bw_RunWorkerCompletedR;
            selectedFilepathR = ofdialog.FileName;
            bwR.RunWorkerAsync(selectedFilepathR);
        }

        void ofdialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isPlaying)
            {
                GMBToggleTimer();
            }
            System.Windows.Forms.OpenFileDialog ofdialog = (System.Windows.Forms.OpenFileDialog)sender;
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += BGWorker;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            selectedFilepathL = ofdialog.FileName;
            bw.RunWorkerAsync(selectedFilepathL);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            wViewer.WaveStream = new NAudio.Wave.WaveFileReader(selectedFilepathL);
            wViewer.StartPosition = 0;
            if (isPlaying)
            {
                GMBToggleTimer();
            }
        }

        void bw_RunWorkerCompletedR(object sender, RunWorkerCompletedEventArgs e)
        {
            wViewerR.WaveStream = new NAudio.Wave.WaveFileReader(selectedFilepathR);
            wViewerR.StartPosition = 0;
            if (isPlaying)
            {
                GMBToggleTimer();
            }
        }

        void BGWorker(object sender, DoWorkEventArgs e)
        {
            //throw new NotImplementedException();
            if (audioPlayer != null)
            {
                audioPlayer.LoadAudioDataL(selectedFilepathL);
            }
        }

        void BGWorkerR(object sender, DoWorkEventArgs e)
        {
            if (audioPlayer != null)
            {
                audioPlayer.LoadAudioDataR(selectedFilepathR);
            }
        }

        private void GMBVolumeSliderValueChangedL(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            float newVal = (float)e.NewValue;
            audioPlayer.VolumeL = newVal;
            //gain_Val.Content = newVal.ToString();
        }

        private void GMBXYPadMouseDown(object sender, MouseButtonEventArgs e)
        {
            //userChangingFilter = true;
            xyPadInUse = true;
        }

        private void GMBXYPadMouseUp(object sender, MouseButtonEventArgs e)
        {
            //userChangingFilter = false;
            xyPadInUse = false;
        }

        private void GMBXYPadMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!xyPadInUse)
            {
                return;
            }

            Point p = Mouse.GetPosition(XYPad);
            double posX = p.X;
            double posY = p.Y;

            //Lbl_ProjSummary.Content = "X: " + posX.ToString("#.#") + "Y: " + posY.ToString("#.#");
            
            //Do the necessary conversions
            GMBNormalizeXYValues(ref posX, ref posY);
            //GMBConvertFractionToFrequencyRange(ref posX);
            //FilterCutoffFrequency = (float)posX;
            FilterCutoffFrequency = (float)GMBLinearScale(posX, 0, 1, 20, 20000);
            FilterQ = (float)GMBLinearScale(posY, 0, 1, 0.707, 10);
        }

        private void GMBFilterValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioPlayer.FilterCutoff = (float)e.NewValue;
        }

        private void GMBFilterQValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            audioPlayer.FilterQ = (float)e.NewValue;
        }

        private void GMBFilterCutoffValueChanged(double newFcVal)
        {
            audioPlayer.FilterCutoff = (float)newFcVal;
        }

        private void GMBFilterQValueChanged(double newQVal)
        {
            audioPlayer.FilterQ = (float)newQVal;
        }

        private void GMBNormalizeXYValues(ref double x, ref double y)
        {
            x = x / XYPad.ActualWidth;
            y = y / XYPad.ActualHeight;
        }

        private void GMBConvertFractionToFrequencyRange(ref double x)
        {   
            //This function converts a "normalized" value of x (0 <= x <= 1) to a value within the frequency range
            x = (x / 19980) + 20;
        }

        private double GMBLinearScale(double x, double in_low, double in_hi, double out_low, double out_hi)
        {  
            //PRE: in_low <= x <= in_hi
            double in_range = in_hi - in_low;
            double out_range = out_hi - out_low;
            double x_norm = (x - in_low) / in_range;
    
            x_norm *= out_range;
            x_norm += out_low; 
            
            return x_norm;    
        }

        private void GMBXFadeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

        private void GMBNextButtonClickedL(object sender, RoutedEventArgs e)
        {
            if (wViewer != null)
            {
                wViewer.StartPosition += 500;
            }
            wViewer.Refresh();
        }

        private void GMBOnPosLChanged(ulong newPos)
        {
            wViewer.StartPosition = (int)newPos;
        }

        private void TestArea()
        {
            audioPlayer.PosChangedL_RegisterCallback(null);
        }
        
     }




    
}

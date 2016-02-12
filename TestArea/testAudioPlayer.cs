using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;

namespace TestArea 
{
    class testAudioPlayer : IWaveformPlayer, INotifyPropertyChanged
    {

        
        public testAudioPlayer(string filepath)
        {
               waveformData = new float[44100 * 10];
               float omega = 2 * (float)Math.PI * 50;
               float N = 44100;
               //float partial = (float)(Math.Sin(omega * i/ N));

               /******************Test Area**************************/
               float x = -7.5F;
               float y = 2.0F;
               float z = GMBDmath.GMBModf(x, y);

               /******************End Test Area***********************/


               for (int i = 0; i < 44100 * 2; ++i)
               {
                   waveformData[i] = GMBDmath.GMBModf((float)(Math.Sin(omega * i / (float)N)), (float)(2 * Math.PI));
               }

               ChannelLength = 44100.0;

        }




        /*********************Event Handlers******************************/
        private void NotifyPropertyChanged([CallerMemberName] String propertyName="")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        

        /*********************Properties*********************************/
        public double ChannelLength {get{return channelLength;} set {channelLength = value; NotifyPropertyChanged();}}
        public double ChannelPosition {get {return channelPosition;} set {channelPosition = value; NotifyPropertyChanged();}}
        public bool IsPlaying {get {return isPlaying;}}
        public TimeSpan SelectionBegin {get {return selectionBegin;} set {selectionBegin = value; NotifyPropertyChanged();}}
        public TimeSpan SelectionEnd { get { return selectionEnd; } set { selectionEnd = value; NotifyPropertyChanged();} }
        public float[] WaveformData {get {return waveformData;} set {waveformData = value;NotifyPropertyChanged();}}

        /***********************Graham's Variables*********************************/
        private double channelLength;
        private double channelPosition;             //This is what I would think of as an playback "index" (for a single channel)
        private bool isPlaying;
        private TimeSpan selectionBegin;
        private TimeSpan selectionEnd;
        private float[] waveformData;

        /*********************Events****************************************/
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

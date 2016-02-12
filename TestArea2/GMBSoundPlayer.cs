using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFSoundVisualizationLib;
using ATLProject2Lib;
using System.Runtime.CompilerServices;

namespace TestArea2
{
    class GMBSoundPlayer : IWaveformPlayer, ISoundPlayer
    {

        private IGMBSimpleObj sourcePlayer;
        /*********************Properties*********************************/
        public double ChannelLength { get { return channelLength; } set { channelLength = value; NotifyPropertyChanged(); } }
        public double ChannelPosition { get { return channelPosition; } set { channelPosition = value; NotifyPropertyChanged(); } }
        public bool IsPlaying { get { return this.isPlaying; }
            protected set
            {
                bool oldValue = isPlaying;
                isPlaying = value;
                if (oldValue != isPlaying)
                    NotifyPropertyChanged("IsPlaying");
                //positionTimer.IsEnabled = value;
            }
         }
        public TimeSpan SelectionBegin { get { return selectionBegin; } set { selectionBegin = value; NotifyPropertyChanged(); } }
        public TimeSpan SelectionEnd { get { return selectionEnd; } set { selectionEnd = value; NotifyPropertyChanged(); } }
        //public float[] WaveformData { get { return waveformData; } set { waveformData = value; NotifyPropertyChanged(); } }

        /***********************Graham's Variables*********************************/
        private double channelLength;
        private double channelPosition;             //This is what I would think of as an playback "index" (for a single channel)
        private bool isPlaying;
        private TimeSpan selectionBegin;
        private TimeSpan selectionEnd;
        private float[] waveformData;

        /*********************Events****************************************/
        public event PropertyChangedEventHandler PropertyChanged;

        /*********************Methods***************************************/
        public GMBSoundPlayer(IGMBSimpleObj _sourcePlayer)
        {
            sourcePlayer = _sourcePlayer;
            isPlaying = false;
        }

        public void LoadAudioDataL(string path)
        {
            sourcePlayer.LoadAudioDataL(path);
            
        }

        public void LoadAudioDataR(string path)
        {
            sourcePlayer.LoadAudioDataR(path);
        }

        public void  Play_Pause()
        {
            sourcePlayer.Play_Pause();
            isPlaying = !isPlaying;
        } 

        public void Run()
        {
            sourcePlayer.Run();
        }

        public void getAudioData(float[] inArr)
        {
            int data_size = sourcePlayer.AudioDataSizeL;
            unsafe
            {
                fixed (float* pArr = inArr)
                {
                    sourcePlayer.GetAudioDataL(*pArr);
                }
            }
        }


        //public event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        //{
        //    add { throw new NotImplementedException(); }
        //    remove { throw new NotImplementedException(); }
        //}

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        bool ISoundPlayer.IsPlaying
        {
            get { return this.isPlaying; }
        }

         //private void NotifyPropertyChanged(String info)
         //{
         //    if (PropertyChanged != null)
         //    {
         //        PropertyChanged(this, new PropertyChangedEventArgs(info));
         //    }
         //}

        double IWaveformPlayer.ChannelLength
        {
            get { throw new NotImplementedException(); }
        }

        double IWaveformPlayer.ChannelPosition
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        TimeSpan IWaveformPlayer.SelectionBegin
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        TimeSpan IWaveformPlayer.SelectionEnd
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        float[] IWaveformPlayer.WaveformData
        {
            get
            {
                this.waveformData = new float[sourcePlayer.AudioDataSizeL];
                unsafe
                {
                    fixed (float* pArr = waveformData)
                    {
                        sourcePlayer.GetAudioDataL(*pArr);
                    }
                }
                PropertyChanged(this, new PropertyChangedEventArgs("WaveformData"));
                return waveformData;
            }
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add {  }
            remove { }
        }
    }
}

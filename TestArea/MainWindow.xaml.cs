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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestArea
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            player = new testAudioPlayer("Test");
            WaveformTimelineDisplay.RegisterSoundPlayer(player);
        }

        private void GMBInit(object sender, RoutedEventArgs e)
        {
            
            //WaveformTimelineDisplay = new WPFSoundVisualizationLib.WaveformTimeline();
            
        }

       

        private testAudioPlayer player;
        private PlaylistWindowTest plWindow;

        private void Btn_Prev_Click(object sender, RoutedEventArgs e)
        {
            plWindow = new PlaylistWindowTest();
            plWindow.Show();
            
        }
    }
}

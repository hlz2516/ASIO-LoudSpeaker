using NAudio.Wave;
using System.Runtime.InteropServices;
using System.Windows;

namespace ASIO_LoudSpeaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AsioOut asioOut;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            var drivers = AsioOut.GetDriverNames();
            asioOut = new AsioOut(drivers[0]);
            var bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(48000, 4));
            asioOut.InitRecordAndPlayback(bufferedWaveProvider, 2, 48000);
            asioOut.AudioAvailable += AsioOut_AudioAvailable;
            asioOut.Play();
        }

        private void AsioOut_AudioAvailable(object? sender, AsioAudioAvailableEventArgs e)
        {
            int index = 0;
            byte[] buf = new byte[e.SamplesPerBuffer * sizeof(int)];
            for (int i = 0; i < e.InputBuffers.Length; i++)
            {
                Marshal.Copy(e.InputBuffers[i], buf, 0, e.SamplesPerBuffer * sizeof(int));
                Marshal.Copy(buf, 0, e.OutputBuffers[index++], e.SamplesPerBuffer * sizeof(int));
                Marshal.Copy(buf, 0, e.OutputBuffers[index++], e.SamplesPerBuffer * sizeof(int));
            }
            e.WrittenToOutputBuffers = true;
        }
    }
}
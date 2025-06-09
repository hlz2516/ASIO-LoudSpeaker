using NAudio.Wave;
using NAudio.Wave.Asio;
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
            int bufferSize = e.SamplesPerBuffer * sizeof(int); //默认内存大小
            switch (e.AsioSampleType)
            {
                case AsioSampleType.Int16MSB:
                case AsioSampleType.Int16LSB:
                    bufferSize = e.SamplesPerBuffer * sizeof(Int16);
                    break;
                case AsioSampleType.Int24MSB:
                case AsioSampleType.Int32MSB:
                case AsioSampleType.Int32MSB16:
                case AsioSampleType.Int32MSB18:
                case AsioSampleType.Int32MSB20:
                case AsioSampleType.Int32MSB24:
                case AsioSampleType.Int24LSB:
                case AsioSampleType.Int32LSB:
                case AsioSampleType.Int32LSB16:
                case AsioSampleType.Int32LSB18:
                case AsioSampleType.Int32LSB20:
                case AsioSampleType.Int32LSB24:
                    bufferSize = e.SamplesPerBuffer * sizeof(int);
                    break;
                case AsioSampleType.Float32MSB:
                case AsioSampleType.Float32LSB:
                    bufferSize = e.SamplesPerBuffer * sizeof(float);
                    break;
                case AsioSampleType.Float64MSB:
                case AsioSampleType.Float64LSB:
                    bufferSize = e.SamplesPerBuffer * sizeof(double);
                    break;
                case AsioSampleType.DSDInt8LSB1: //追求极致音质的专业级或高端消费级设备支持这一类采样类型，本应用中不考虑
                case AsioSampleType.DSDInt8MSB1:
                case AsioSampleType.DSDInt8NER8:
                    throw new NotSupportedException("不支持DSD采样类型的设备，请使用其他采样类型的设备");
                default:
                    break;
            }
            byte[] buf = new byte[bufferSize];
            //int index = 0;
            //for (int i = 0; i < e.InputBuffers.Length; i++)
            //{
            //    Marshal.Copy(e.InputBuffers[i], buf, 0, bufferSize);
            //    Marshal.Copy(buf, 0, e.OutputBuffers[index++], bufferSize);
            //    Marshal.Copy(buf, 0, e.OutputBuffers[index++], bufferSize);
            //}
            //分配一半的输入通道数据写入到一半的输出通道，例如输入通道1，2，输出通道1，2，3，4，则输入通道1写入输出通道1，2，输入通道2写入输出通道3，4
            int i =0,j=0;
            for (; i < e.InputBuffers.Length / 2; i++)
            {
                Marshal.Copy(e.InputBuffers[i], buf, 0, bufferSize);
                for (; j < e.OutputBuffers.Length / 2; j++)
                {
                    Marshal.Copy(buf, 0, e.OutputBuffers[j], bufferSize);
                }
            }
            for (; i < e.InputBuffers.Length; i++)
            {
                Marshal.Copy(e.InputBuffers[i], buf, 0, bufferSize);
                for (; j < e.OutputBuffers.Length; j++)
                {
                    Marshal.Copy(buf, 0, e.OutputBuffers[j], bufferSize);
                }
            }
            e.WrittenToOutputBuffers = true;
        }
    }
}
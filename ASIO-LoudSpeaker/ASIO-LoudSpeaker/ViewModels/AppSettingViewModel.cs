using ASIO_LoudSpeaker.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Collections.ObjectModel;
using System.IO;

namespace ASIO_LoudSpeaker.ViewModels
{
    public partial class AppSettingViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenControlPanelCommand))]
        private string selectedASIODriver;
        partial void OnSelectedASIODriverChanged(string value)
        {
            AsioDriver = new AsioOut(value);
        }
        [ObservableProperty]
        private int inputChannelCount;
        [ObservableProperty]
        private int inputChannelOffset;
        [ObservableProperty]
        private int inputSampleRate;
        [ObservableProperty]
        private int outputChannelCount;
        [ObservableProperty]
        private int outputChannelOffset;
        [ObservableProperty]
        private int outputSampleRate;
        [ObservableProperty]
        private AsioOut asioDriver;
        public ObservableCollection<string> ASIODrivers { get; } = new(AsioOut.GetDriverNames());
        private AudioPlaybackEngine playbackEngine;
        public AppSettingViewModel()
        {
            if (ASIODrivers.Contains(Config.Default.ASIODriverName))
            {
                LoadConfig();
            }
        }

        [RelayCommand(CanExecute =nameof(CanOpenControlPanel))]
        private void OpenControlPanel()
        {
            if (AsioDriver != null)
            {
                AsioDriver.ShowControlPanel();
            }
        }

        [RelayCommand]
        private void InputTest()
        {

        }

        [RelayCommand]
        private void OutputTest()
        {
            if (playbackEngine is null)
            {
                playbackEngine = new AudioPlaybackEngine(SelectedASIODriver,OutputSampleRate,OutputChannelCount);
            }

            string testAudioPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/Audio/c1.wav");
            playbackEngine.PlaySound(testAudioPath);
        }

        private bool CanOpenControlPanel() => !string.IsNullOrEmpty(SelectedASIODriver) && ASIODrivers.Contains(SelectedASIODriver);

        [RelayCommand]
        private void SaveSettings()
        {
            Config.Default.ASIODriverName = SelectedASIODriver;
            Config.Default.InputChannelOffset = InputChannelOffset;
            Config.Default.InputChannelCount = InputChannelCount;
            Config.Default.InputSampleRate = InputSampleRate;
            Config.Default.OutputChannelOffset = OutputChannelOffset;
            Config.Default.OutputChannelCount = OutputChannelCount;
            Config.Default.Save();
        }

        [RelayCommand]
        private void Cancel()
        {
            if (ASIODrivers.Contains(Config.Default.ASIODriverName))
            {
                LoadConfig();
            }
        }

        private void LoadConfig()
        {
            SelectedASIODriver = Config.Default.ASIODriverName;
            InputChannelOffset = Config.Default.InputChannelOffset;
            inputChannelCount = Config.Default.InputChannelCount;
            InputSampleRate = Config.Default.InputSampleRate;
            OutputChannelOffset = Config.Default.OutputChannelOffset;
            OutputChannelCount = Config.Default.OutputChannelCount;
        }
    }
}

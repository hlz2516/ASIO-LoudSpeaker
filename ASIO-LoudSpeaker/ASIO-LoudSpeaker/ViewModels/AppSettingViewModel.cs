using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASIO_LoudSpeaker.ViewModels
{
    public partial class AppSettingViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenControlPanelCommand))]
        [NotifyPropertyChangedFor(nameof(AsioDriver))]
        private string selectedASIODriver;
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

        private AsioOut asioDriver;
        public AsioOut AsioDriver
        {
            get => asioDriver;
            set
            {
                asioDriver = new AsioOut(SelectedASIODriver);
                SetProperty(ref asioDriver, value);
            }
        }
        public ObservableCollection<string> ASIODrivers { get; } = new(AsioOut.GetDriverNames());
        public AppSettingViewModel()
        {
            if (ASIODrivers.Contains(Config.Default.ASIODriverName))
            {
                SelectedASIODriver = Config.Default.ASIODriverName;
                InputChannelOffset = Config.Default.InputChannelOffset;
                inputChannelCount = Config.Default.InputChannelCount;
                InputSampleRate = Config.Default.InputSampleRate;
                OutputChannelOffset = Config.Default.OutputChannelOffset;
                OutputChannelCount = Config.Default.OutputChannelCount;
                OutputSampleRate = Config.Default.OutputSampleRate;
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

        private bool CanOpenControlPanel() => !string.IsNullOrEmpty(SelectedASIODriver) && ASIODrivers.Contains(SelectedASIODriver);

        [RelayCommand]
        private void SaveSettings()
        {

        }

        [RelayCommand]
        private void Cancel()
        {

        }
    }
}

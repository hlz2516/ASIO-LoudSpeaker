using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
namespace MvvmSampleMAUI.Views.Widgets;

public abstract class BaseContentView<TViewModel> : UserControl where TViewModel : ObservableObject
{
    protected BaseContentView(TViewModel viewModel)
    {
        DataContext = viewModel;
    }

    public TViewModel BindingContext => (TViewModel)DataContext;
}
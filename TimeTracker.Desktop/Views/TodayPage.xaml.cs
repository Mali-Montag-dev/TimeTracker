using System.Windows.Controls;
using TimeTracker.Desktop.ViewModels;

namespace TimeTracker.Desktop.Views
{
    public partial class TodayPage : Page
    {
        private readonly TodayViewModel _viewModel = new TodayViewModel();

        public TodayPage()
        {
            InitializeComponent();
            DataContext = _viewModel;

            Loaded += (_, _) => _viewModel.Start();
            Unloaded += (_, _) => _viewModel.Stop();
        }
    }
}
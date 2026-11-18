using System.Windows.Controls;
using TimeTracker.Desktop.ViewModels;

namespace TimeTracker.Desktop.Views
{
    public partial class AppsPage : Page
    {
        private readonly AppsViewModel _viewModel = new AppsViewModel();

        public AppsPage()
        {
            InitializeComponent();
            DataContext = _viewModel;

            Loaded += async (_, _) => await _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
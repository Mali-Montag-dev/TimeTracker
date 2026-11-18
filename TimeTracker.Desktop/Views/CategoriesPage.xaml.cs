using System.Windows.Controls;
using TimeTracker.Desktop.ViewModels;

namespace TimeTracker.Desktop.Views
{
    public partial class CategoriesPage : Page
    {
        private readonly CategoriesViewModel _viewModel = new CategoriesViewModel();

        public CategoriesPage()
        {
            InitializeComponent();
            DataContext = _viewModel;

            Loaded += async (_, _) => await _viewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
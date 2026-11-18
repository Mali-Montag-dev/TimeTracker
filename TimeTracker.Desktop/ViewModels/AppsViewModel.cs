using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenTK.Audio.OpenAL;
using System.Collections.ObjectModel;
using System.Net.Http;
using TimeTracker.Desktop.Services;

namespace TimeTracker.Desktop.ViewModels
{
    // ===== ה-ViewModel של המסך =====
    public partial class AppsViewModel : ObservableObject
    {
        private readonly List<AppItemViewModel> _allApps = new List<AppItemViewModel>();

        public ObservableCollection<AppItemViewModel> Apps { get; } = new ObservableCollection<AppItemViewModel>();
        public ObservableCollection<CategoryOption> CategoryOptions { get; } = new ObservableCollection<CategoryOption>();

        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _hasError;

        [RelayCommand]
        private async Task LoadAsync()
        {
            try
            {
                HasError = false;

                var categories = await App.Api.GetCategoriesAsync();
                var apps = await App.Api.GetAppsAsync();

                // אפשרויות הבחירה: "ללא קטגוריה" ואחריה כל הקטגוריות
                CategoryOptions.Clear();
                CategoryOptions.Add(CategoryOption.None);
                foreach (var c in categories)
                    CategoryOptions.Add(new CategoryOption(c.Id, c.Name, c.Color));

                // שורה לכל תוכנה
                _allApps.Clear();
                foreach (var app in apps)
                {
                    var selected = CategoryOptions.FirstOrDefault(o => o.Id == app.CategoryId)
                                   ?? CategoryOption.None;

                    _allApps.Add(new AppItemViewModel(
                        app.Id, app.DisplayName, app.ProcessName, selected, SaveCategoryAsync));
                }

                ApplyFilter();
            }
            catch (HttpRequestException)
            {
                ShowError("ה-API לא זמין. ודאי שהוא פועל.");
            }
        }

        // מופעל אוטומטית בכל פעם שהטקסט בתיבת החיפוש משתנה
        partial void OnSearchTextChanged(string value) => ApplyFilter();

        private void ApplyFilter()
        {
            Apps.Clear();
            foreach (var app in _allApps.Where(a => a.Matches(SearchText)))
                Apps.Add(app);
        }

        private async Task SaveCategoryAsync(int appId, int? categoryId)
        {
            try
            {
                HasError = false;
                await App.Api.AssignCategoryAsync(appId, categoryId);
            }
            catch (ApiException ex)
            {
                ShowError(ex.Message);
            }
            catch (HttpRequestException)
            {
                ShowError("ה-API לא זמין. ודאי שהוא פועל.");
            }
        }

        private void ShowError(string message)
        {
            ErrorMessage = message;
            HasError = true;
        }
    }

    // ===== ViewModel לשורה אחת ברשימה =====
    public partial class AppItemViewModel : ObservableObject
    {
        private readonly Func<int, int?, Task> _save;
        private readonly bool _ready;

        public int Id { get; }
        public string DisplayName { get; }
        public string ProcessName { get; }

        [ObservableProperty] private CategoryOption _selectedCategory = CategoryOption.None;

        public AppItemViewModel(int id, string displayName, string processName,
                                CategoryOption selected, Func<int, int?, Task> save)
        {
            Id = id;
            DisplayName = displayName;
            ProcessName = processName;
            _save = save;

            SelectedCategory = selected;
            _ready = true;
        }

        // מופעל אוטומטית כשבוחרים קטגוריה אחרת ברשימה הנפתחת
        partial void OnSelectedCategoryChanged(CategoryOption value)
        {
            if (_ready && value != null)
                _ = _save(Id, value.Id);
        }

        public bool Matches(string text)
        {
            return string.IsNullOrWhiteSpace(text)
                || DisplayName.Contains(text, StringComparison.OrdinalIgnoreCase)
                || ProcessName.Contains(text, StringComparison.OrdinalIgnoreCase);
        }
    }

    // ===== אפשרות ברשימה הנפתחת =====
    public record CategoryOption(int? Id, string Name, string Color)
    {
        public static CategoryOption None { get; } = new CategoryOption(null, "ללא קטגוריה", "#9E9E9E");
    }
}